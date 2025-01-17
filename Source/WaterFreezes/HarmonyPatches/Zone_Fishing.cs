using VCE_Fishing.Options;
using Verse;

namespace WF.Harmony_Patches;

internal static class Zone_Fishing
{
    public static void Postfix_AllowFishing(object __instance, ref bool __result)
    {
        if (!__result)
        {
            return;
        }

        var zone = (VCE_Fishing.Zone_Fishing)__instance;
        if (isFrozen(zone))
        {
            __result = false;
        }
    }

    public static void Postfix_GetInspectString(object __instance, ref string __result)
    {
        var zone = (VCE_Fishing.Zone_Fishing)__instance;
        if (isFrozen(zone))
        {
            __result += "\n" + "WFM.frozen".Translate();
        }
    }

    private static bool isFrozen(Zone zone)
    {
        var map = zone.Map;
        var comp = WaterFreezesCompCache.GetFor(map);
        if (comp is not { Initialized: true })
        {
            return false;
        }

        if (!comp.IceDepthGrid.ContainsAny(thinckness => thinckness > 0))
        {
            return false;
        }

        var cells = zone.Cells;
        var cellCount = cells.Count;
        var minCount = VCE_Fishing_Settings.VCEF_minimumZoneSize;

        foreach (var intVec3 in cells)
        {
            var iceDepth = comp.IceDepthGrid[map.cellIndices.CellToIndex(intVec3)];
            if (iceDepth > 0)
            {
                cellCount--;
            }

            if (cellCount >= minCount)
            {
                continue;
            }

            return true;
        }

        return false;
    }
}