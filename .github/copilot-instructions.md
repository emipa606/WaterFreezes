# Water Freezes (Continued) - GitHub Copilot Instructions

## Mod Overview and Purpose

The "Water Freezes (Continued)" mod enhances the realism of water bodies in RimWorld by introducing dynamic freezing and thawing mechanics. This mod, an update of UdderlyEvelyn's original, aims to simulate realistic thermodynamics while maintaining optimal game performance.

## Key Features and Systems

- **Dynamic Water Freezing:**
  - Lakes, marshes, and rivers freeze during cold weather.
  - Oceans can freeze at very low temperatures, enabled via mod options.

- **Realistic Thermodynamics:**
  - Water and ice depth tracking to simulate freezing and thawing transformations.
  - Ice melting rates based on thickness, with freezing initiating from the outer edge.

- **Building Interactions:**
  - Structures unsuitable for ice face destruction or breakdown during freeze-thaw cycles.
  - Bridged buildings, including those from mods, remain unaffected.

- **Resource and Environmental Management:**
  - Soil Relocation Framework integration for ice collection and water digging.
  - Ice can be collected for cooling purposes—useful even during solar flares.

- **Planned Features:**
  - Ice skating as a recreation.
  - Sculptures made from ice with cooling properties.

- **Compatibility Enhancements:**
  - Works with Vanilla Fishing Expanded and other mod dependencies like the Soil Relocation Framework.

## Coding Patterns and Conventions

- **Class Structure:**
  - Utilizes static helper classes (e.g., `IceDefs`, `WaterDefs`) for modular and reusable code.
  - Implements `MapComponent_WaterFreezes` for managing map-wide water and ice interactions.

- **Convention Adherence:**
  - Consistent naming conventions for classes and methods.
  - Use of namespaces to group related functionalities.

## XML Integration

- Patches and extensions to existing game XML definitions via custom classes such as `TerrainDefExtensions` and `TerrainExtension_WaterStats`.
- XML customizations allow seamless interaction with base game and other mods’ terrain definitions.

## Harmony Patching

- Employs Harmony patches to extend or override base game functionality without altering the original source code.
- `ToggleablePatch` system implemented to provide modular patch application and removal mechanisms.

## Suggestions for Copilot

- **Completion Patterns:**
  - Assist in generating `public` or `internal` classes based on existing patterns.
  - Facilitate method suggestions within patched classes.

- **Harmony Integration Assistance:**
  - Recommend Harmony patch structures based on similar existing patches.

- **XML Definition Extensions:**
  - Suggest XML patches or extensions for new terrain or building behaviors congruent with existing integrations.

- **Optimization Code Hints:**
  - Identify potential performance bottlenecks in iterative methods and suggest optimizations.

- **Debugging and Mod Debug Tools:**
  - Suggest debug tools and methods aligned with `DebugActionsWaterFreezes`.

By following these instructions, GitHub Copilot can better assist in supporting the development and maintenance of the "Water Freezes (Continued)" mod by providing relevant code suggestions and ensuring adherence to the mod’s coding standards.
