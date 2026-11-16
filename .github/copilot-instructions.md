# GitHub Copilot Instructions for Water Freezes (Continued) Mod

## Mod Overview and Purpose
The Water Freezes (Continued) mod for RimWorld is an update to UdderlyEvelyn's original Water Freezes mod. This mod introduces the ability for lakes, marshes, rivers, and oceans to freeze when it is sufficiently cold outside. It provides a more realistic thermodynamic simulation for freezing and thawing processes while ensuring the game remains fast and smooth. The mod also adds compatibility with Vanilla Fishing Expanded, allowing players to fish through frozen lakes.

## Key Features and Systems
### Freezing and Thawing
- **Freezing Process**: Lakes, marshes, and rivers freeze when temperatures drop. The freezing process is realistic, with thicker ice melting slower than thinner ice.
- **Thawing Process**: Ice melts gradually over time, preventing lag and stuttering.
- **Thermodynamic Simulation**: The simulation estimates lake depth based on distance from land and calculates freezing from the outside in.

### Building Interactions
- **Buildings on Water/Ice**: Buildings that cannot exist on ice but can on water will break down when water freezes under them. Conversely, buildings that can exist on ice but are built on water will be destroyed when the terrain thaws.
- **Bridges**: Buildings on bridges, including modded ones, are immune to freezing and thawing effects.

### Natural Water Refill
- **Natural Lakes**: Natural lakes refill in the spring if their ice is removed. Artificial lakes do not refill naturally.

### Debug Tools
- **Clear Natural Water Status**: Clears the natural water status of a cell.
- **Reinitialize Map Component**: Reinitializes the map component to remove all ice.

### Compatibility
- **Soil Relocation Framework**: Interoperates with Soil Relocation Framework.
- **Vanilla Fishing Expanded**: Allows fishing through frozen lakes.
- **Multiplayer Mod**: Works with the Multiplayer mod.
- **Terrain System Overhaul**: Compatible with Terrain System Overhaul.
- **Terraforming Mods**: Natural water terrain will revert if altered by terraforming mods.

## Coding Patterns and Conventions
### C# Files
- **DebugActionsWaterFreezes.cs**: Contains debug actions for clearing natural water status, setting ice and water depth, and reinitializing the map component.
- **Defs/IceDefs.cs**: Defines ice-related definitions.
- **Defs/WaterDefs.cs**: Defines water-related definitions.
- **ErrorLevel.cs**: Manages error levels for debugging purposes.
- **HarmonyPatches/CompTerrainPumpDry_AffectCell.cs**: Contains harmony patches for the `CompTerrainPumpDry_AffectCell` class.

### XML Files
- **Terrain.xml**: Defines terrain-related definitions.
- **RFReworkIceFishable.xml**: Patches for Soil Relocation Framework.
- **VanillaWaterSupport.xml**: Patches for Vanilla Fishing Expanded.

## XML Integration
The mod uses XML files to define terrain, ice, and water-related definitions. The XML files are located in the `Defs` directory and are used to integrate with the game's existing systems.

Example XML snippet:
xml
<TerrainDef>
    <defName>Ice</defName>
    <label>Ice</label>
    <description>Covered with a layer of frozen water.</description>
    <graphicData>
        <texPath>Things/Terrain/Ice</texPath>
        <graphicClass>Graphic_Single</graphicClass>
    </graphicData>
    <statBases>
        <statDef>WalkSpeed</statDef>
        <value>0.33</value>
    </statBases>
</TerrainDef>


## Harmony Patching
The mod uses Harmony patching to modify the behavior of existing classes and methods. Harmony patches are located in the `HarmonyPatches` directory and are used to add, remove, or replace code in the game's assemblies.

Example Harmony patch:
csharp
[HarmonyPatch(typeof(CompTerrainPumpDry), "AffectCell")]
public static class CompTerrainPumpDry_AffectCell_Patch
{
    public static void Postfix(CompTerrainPumpDry __instance, IntVec3 c)
    {
        // Custom code to handle freezing and thawing
    }
}


## Suggestions for Copilot
### C# Development
- **Debug Actions**: Use Copilot to generate debug actions for managing natural water status, setting ice and water depth, and reinitializing the map component.
- **Harmony Patches**: Use Copilot to generate harmony patches for existing classes and methods, ensuring compatibility with the game's updates.
- **Terrain and Water Definitions**: Use Copilot to generate XML definitions for terrain, ice, and water-related entities.

### XML Integration
- **Terrain Definitions**: Use Copilot to create XML snippets for defining new terrain types, such as ice and frozen water.
- **Patches**: Use Copilot to generate XML patches for modifying existing terrain and water behaviors.

### General Suggestions
- **Code Formatting**: Ensure that all generated code adheres to the RimWorld coding conventions, including proper naming conventions and indentation.
- **Testing**: After generating code, thoroughly test the mod to ensure compatibility and functionality.
- **Documentation**: Update the mod's documentation to reflect any new features or changes made using Copilot.

By following these guidelines, developers can effectively utilize GitHub Copilot to enhance the Water Freezes (Continued) mod, ensuring a realistic and enjoyable freezing and thawing experience in RimWorld.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

