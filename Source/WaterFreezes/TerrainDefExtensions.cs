using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace WF;

public static class TerrainDefExtensions
{
    private static readonly Dictionary<TerrainDef, bool> bridgeCache = new();

    extension(TerrainDef def)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsBridge()
        {
            if (!bridgeCache.ContainsKey(def))
            {
                bridgeCache[def] = def.bridge || def.label.ToLowerInvariant().Contains("bridge") ||
                                   def.defName.ToLowerInvariant().Contains("bridge");
            }

            return bridgeCache[def];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFreezableWater()
        {
            return WaterFreezesStatCache.FreezableWater.Contains(def.defName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsThawableIce()
        {
            return WaterFreezesStatCache.ThawableIce.Contains(def.defName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsShallowDepth()
        {
            return def == TerrainDefOf.WaterShallow || def == TerrainDefOf.WaterMovingShallow;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsDeepDepth()
        {
            return def == TerrainDefOf.WaterDeep || def == TerrainDefOf.WaterMovingChestDeep;
        }
    }
}