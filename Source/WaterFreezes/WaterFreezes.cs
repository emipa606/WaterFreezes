using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Zone_Fishing = WF.Harmony_Patches.Zone_Fishing;

namespace WF;

[StaticConstructorOnStartup]
public static class WaterFreezes
{
    [ToggleablePatch] public static ToggleablePatch<ThingDef> WatermillIsFlickablePatch = new()
    {
        Name = "Watermill Is Flickable",
        Enabled = true,
        TargetDefName = "WatermillGenerator",
        Patch = (_, def) => { def.comps.Add(new CompProperties_Flickable()); },
        Unpatch = (_, def) => { def.comps.RemoveAll(comp => comp is CompProperties_Flickable); }
    };

    [ToggleablePatch] public static ToggleablePatch<ThingDef> VPEAdvancedWatermillIsFlickablePatch =
        new()
        {
            Name = "VPE Advanced Watermill Is Flickable",
            Enabled = true,
            TargetModID = "VanillaExpanded.VFEPower",
            TargetDefName = "VFE_AdvancedWatermillGenerator",
            Patch = (_, def) => { def.comps.Add(new CompProperties_Flickable()); },
            Unpatch = (_, def) => { def.comps.RemoveAll(c => c is CompProperties_Flickable); }
        };

    [ToggleablePatch] public static ToggleablePatch<ThingDef> VPETidalGeneratorIsFlickablePatch =
        new()
        {
            Name = "VPE Tidal Generator Is Flickable",
            Enabled = true,
            TargetModID = "VanillaExpanded.VFEPower",
            TargetDefName = "VFE_TidalGenerator",
            Patch = (_, def) => { def.comps.Add(new CompProperties_Flickable()); },
            Unpatch = (_, def) => { def.comps.RemoveAll(c => c is CompProperties_Flickable); }
        };

    private static readonly Version version = Assembly.GetAssembly(typeof(WaterFreezes)).GetName().Version;
    public static readonly bool UsingVanillaFishingExpanded;

    /// <summary>
    ///     The assembly version of the mod.
    /// </summary>
    private static readonly string Version = $"{version.Major}.{version.Minor}.{version.Build}";

    static WaterFreezes()
    {
        Log("Initializing.");
        ToggleablePatch.ProcessPatches("UdderlyEvelyn.WaterFreezes");
        var harmony = new Harmony("UdderlyEvelyn.WaterFreezes");
        harmony.PatchAll();
        WaterFreezesStatCache.Initialize();
        if (!ModLister.HasActiveModWithName("Vanilla Fishing Expanded"))
        {
            return;
        }

        UsingVanillaFishingExpanded = true;
        Log("Adding compatibility for Vanilla Fishing Expanded");
        harmony.Patch(AccessTools.Method("VCE_Fishing.Zone_Fishing:get_AllowFishing"), postfix:
            new HarmonyMethod(Zone_Fishing.Postfix_AllowFishing));
        harmony.Patch(AccessTools.Method("VCE_Fishing.Zone_Fishing:GetInspectString"), postfix:
            new HarmonyMethod(Zone_Fishing.Postfix_GetInspectString));
    }


    /// <summary>
    ///     Logging function for the mod, prints the message with the appropriate method based on errorLevel, optionally
    ///     ignoring the "stop logging limit".
    /// </summary>
    /// <param name="message">the message to log</param>
    /// <param name="errorLevel">the type of logging method to use</param>
    /// <param name="errorOnceKey">if doing ErrorOnce logging, the unique key to use (defaults to 0)</param>
    /// <param name="ignoreStopLoggingLimit">if true, resets the message count before logging the message</param>
    public static void Log(string message, ErrorLevel errorLevel = ErrorLevel.Message, int errorOnceKey = 0,
        bool ignoreStopLoggingLimit = false)
    {
        if (ignoreStopLoggingLimit)
        {
            Verse.Log.ResetMessageCount();
        }

        var text = $"[Water Freezes {WaterFreezesMod.currentVersion ?? Version}] {message}";
        switch (errorLevel)
        {
            case ErrorLevel.Message:
                Verse.Log.Message(text);
                break;
            case ErrorLevel.Warning:
                Verse.Log.Warning(text);
                break;
            case ErrorLevel.Error:
                Verse.Log.Error(text);
                break;
            case ErrorLevel.ErrorOnce:
                Verse.Log.ErrorOnce(text, errorOnceKey);
                break;
        }
    }
}