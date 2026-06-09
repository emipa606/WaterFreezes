# GitHub Copilot Instructions for Water Freezes (Continued) Mod

## Mod Overview and Purpose
The "Water Freezes (Continued)" mod for RimWorld enhances the environmental realism of the game by introducing dynamic water freezing and thawing mechanics. As a successor to UdderlyEvelyn's original mod, this version utilizes RimWorld 1.6's improved Asset Bundles support to minimize mod size and improve performance. Note that this mod should not be used alongside the Odyssey mod due to overlapping features.

## Key Features and Systems
- **Water Freezing:** Lakes, marshes, and rivers can freeze when temperatures drop. This affects gameplay by influencing available resources and altering terrain accessibility.
- **Water and Ice Depth Tracking:** Tracks the thickness of water and ice, affecting melting and freezing rates with a relatively accurate thermodynamic simulation.
- **Terrain Interaction:** Buildings on ice may break or be destroyed depending on their compatibility with water, impacting construction and resource management decisions.
- **Digable Ice:** Players can dig ice for materials, impacting local environments and providing alternative cooling solutions, such as tribal freezers.
- **Seasonal Dynamics:** Natural lakes refill in spring, contrasting with artificial bodies of water.

## Coding Patterns and Conventions
- The mod adheres to established C# coding conventions relevant to RimWorld modding, ensuring maintainability and compatibility.
- A clear separation of concerns is observed in C# files, with functionalities organized according to gameplay features.
- The project includes debug tools for mod testing, aiding in development and user support.

## XML Integration
- The mod utilizes several XML files to define in-game terrain meta information and compatibility patches.
- XML files provide foundational data for water and ice types, as well as temperature thresholds for freezing and thawing events.
- Custom TerrainDefs are defined to integrate seamlessly with RimWorld's existing mechanics, providing a robust base for feature development.

## Harmony Patching
- Harmony is used to patch existing game functions, allowing for modifications and new behaviors without altering core game files.
- **CompTerrainPumpDry_AffectCell:** This includes both prefix and postfix patches to manage terrain interactions when temperature changes affect water or ice.

## Suggestions for Copilot
- **XML Assistance:** Utilize Copilot to help generate new TerrainDefs or modify existing ones through structured XML editing as new features are developed.
- **C# Debug Tools:** Refine the debug functionalities by automating repetitive tasks or complex setup scenarios.
- **Harmony Patch Development:** Assist in creating new patches for additional game interactions with water freezing, especially concerning unplanned or mod-incompatible buildings on ice.
- **Efficiency Improvements:** Suggest code optimizations and improvements based on existing coding patterns to ensure the mod runs efficiently.

## Conclusion
The "Water Freezes (Continued)" mod is a comprehensive update showcasing dynamic environmental changes in RimWorld. With detailed systems supporting community-driven playstyles, it leverages Asset Bundles and Harmony patches effectively. GitHub Copilot can significantly aid in maintaining and expanding these systems, streamlining development and enhancing user experience.

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

