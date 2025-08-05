using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace WF;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ToggleablePatch : Attribute
{
    private static readonly bool AutoScan = true;
    private static bool _performedPatchScan;
    public static readonly Action<string> MessageLoggingMethod = Log.Message;
    public static Action<string> WarningLoggingMethod = Log.Warning;
    public static readonly Action<string> ErrorLoggingMethod = Log.Error;
    private static readonly List<IToggleablePatch> Patches = [];

    private static void ScanForPatches()
    {
        if (_performedPatchScan)
        {
            return;
        }

        var types = Assembly.GetExecutingAssembly().GetTypes();
        var members = types.SelectMany(type => type.GetMembers(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Static
            )
        );
        var iToggleablePatchType = typeof(IToggleablePatch);
        foreach (var member in members)
        {
            if (!member.HasAttribute<ToggleablePatch>())
            {
                continue;
            }

            switch (member)
            {
                case FieldInfo field when field.FieldType.GetInterfaces().Contains(iToggleablePatchType):
                    Patches.Add((IToggleablePatch)field.GetValue(null));
                    break;
                case FieldInfo field:
                    ErrorLoggingMethod(
                        $"[ToggleablePatch] Field \"{field.Name}\" is marked with ToggleablePatch attribute but does not implement IToggleablePatch.");
                    break;
                case PropertyInfo property when property.PropertyType.GetInterfaces().Contains(iToggleablePatchType):
                    Patches.Add((IToggleablePatch)property.GetValue(null));
                    break;
                case PropertyInfo property:
                    ErrorLoggingMethod(
                        $"[ToggleablePatch] Property \"{property.Name}\" is marked with ToggleablePatch attribute but does not implement IToggleablePatch.");
                    break;
            }
        }

        _performedPatchScan = true;
    }

    public static void AddPatches(params IToggleablePatch[] patches)
    {
        Patches.AddRange(patches);
    }

    /// <summary>
    ///     Process the patches stored in ToggleablePatch.Patches.
    /// </summary>
    /// <param name="modID"></param>
    /// <param name="reason">the reason to process them, optional, shown in logging</param>
    public static void ProcessPatches(string modID, string reason = null)
    {
        if (AutoScan)
        {
            ScanForPatches();
        }

        MessageLoggingMethod(
            $"[ToggleablePatch] Processing {Patches.Count} patches{(reason != null ? $" because {reason}" : "")} for \"{modID}\"..");
        foreach (var patch in Patches)
        {
            patch.Process();
        }
    }
}

public class ToggleablePatch<T> : IToggleablePatch where T : Def
{
    /// <summary>
    ///     List of conflicting mod IDs that this patch will not be applied if are present.
    /// </summary>
    private readonly List<string> ConflictingModIDs = [];

    /// <summary>
    ///     Cache variable for whether the mod that is targeted is installed.
    /// </summary>
    private bool? modInstalled;

    /// <summary>
    ///     The patch code.
    /// </summary>
    public Action<ToggleablePatch<T>, T> Patch;

    /// <summary>
    ///     A space to save data from the patching process to be used by the unpatching process, primarily for restoring
    ///     non-vanilla data in destructive patches.
    /// </summary>
    public object State;

    /// <summary>
    ///     Cache variable for the target def.
    /// </summary>
    private T targetDef;

    /// <summary>
    ///     The def name of the def targeted by this patch.
    /// </summary>
    public string TargetDefName;

    /// <summary>
    ///     The mod ID of the mod that is targeted by this patch.
    /// </summary>
    public string TargetModID;

    /// <summary>
    ///     The unpatch code - this should undo the patch code completely.
    /// </summary>
    public Action<ToggleablePatch<T>, T> Unpatch;

    /// <summary>
    ///     Returns the target as a string in the form of ModID.DefName (DefType.FullName) with the "ModID." missing if no Mod
    ///     ID is assigned (e.g., it's Vanilla or unconditional).
    /// </summary>
    private string TargetDescriptionString =>
        $"{(TargetModID != null ? $"{TargetModID}." : "")}{TargetDefName} ({typeof(T).FullName})";

    /// <summary>
    ///     Whether we can patch this, depends on whether the mod it comes from is installed (always true if it's from
    ///     vanilla).
    /// </summary>
    private bool CanPatch
    {
        get
        {
            foreach (var modID in ConflictingModIDs)
            {
                if (ModLister.GetActiveModWithIdentifier(modID, true) == null) //If mod present.
                {
                    continue;
                }

                ToggleablePatch.MessageLoggingMethod(
                    $"[ToggleablePatch] Skipping patch \"{Name}\" because conflicting mod with ID \"{modID}\" was found.");
                return false; //Can't patch.
            }

            if (TargetModID == null) //If it has a target mod.
            {
                return true; //Return true if it had no target mod ID and no conflicting mods stopped it before now.
            }

            modInstalled ??= ModLister.GetActiveModWithIdentifier(TargetModID, true) != null;

            return modInstalled.Value; //Return true if it is installed, false if not.
        }
    }

    /// <summary>
    ///     Name of the patch.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Whether the patch is enabled or not, dictates where it will be applied or removed when processed.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    ///     Whether the patch is presently applied or not.
    /// </summary>
    public bool Applied { get; private set; }

    /// <summary>
    ///     Apply the patch if possible and necessary.
    /// </summary>
    public void Apply()
    {
        if (CanPatch)
        {
            if (!Applied)
            {
                ToggleablePatch.MessageLoggingMethod(
                    $"[ToggleablePatch] {(Name != null ? $"Applying patch \"{Name}\", patching " : "Patching ")}{TargetDescriptionString}..");
                targetDef ??= DefDatabase<T>.GetNamedSilentFail(TargetDefName);

                if (targetDef == null)
                {
                    ToggleablePatch.MessageLoggingMethod(
                        $"[ToggleablePatch] Skipping application of patch \"{Name}\" because {TargetDefName} cannot be found.");
                    return;
                }

                try
                {
                    Patch(this, targetDef);
                }
                catch (Exception ex)
                {
                    ToggleablePatch.ErrorLoggingMethod(
                        $"[ToggleablePatch] Error {(Name != null ? $"applying patch \"{Name}\"" : "patching ")}. Most likely you have another mod that already patches {TargetDescriptionString}. Remove that mod or disable this patch in the mod options.\n\n{ex}");
                }

                Applied = true; //Set it as applied.
            }
            else
            {
                ToggleablePatch.MessageLoggingMethod(
                    $"[ToggleablePatch] Skipping application of patch \"{Name}\" because it is already applied.");
            }
        }
        else
        {
            ToggleablePatch.MessageLoggingMethod(
                $"[ToggleablePatch] Skipping application of patch \"{Name}\" because it cannot be applied.");
        }
    }

    /// <summary>
    ///     Remove the patch if possible and necessary.
    /// </summary>
    public void Remove()
    {
        if (Applied) //If it's been applied already.
        {
            ToggleablePatch.MessageLoggingMethod(
                $"[ToggleablePatch] {(Name != null ? $"Removing patch \"{Name}\", unpatching " : "Unpatching ")}{TargetDescriptionString}..");
            targetDef ??= DefDatabase<T>.GetNamed(TargetDefName);

            try
            {
                Unpatch(this, targetDef);
            }
            catch (Exception ex)
            {
                ToggleablePatch.ErrorLoggingMethod(
                    $"[ToggleablePatch] Error {(Name != null ? $"removing patch \"{Name}\"" : "unpatching ")}. Most likely you have another mod that already patches {TargetDescriptionString}, and it failed to patch in the first place. Remove that mod or disable this patch in the mod options.\n\n{ex}");
            }

            Applied = false; //Set it as not applied anymore.
        }
        else
        {
            ToggleablePatch.MessageLoggingMethod(
                $"[ToggleablePatch] Skipping removal of patch \"{Name}\" because it is not applied.");
        }
    }
}