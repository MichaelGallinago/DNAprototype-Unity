A game prototype containing:
1. Tile baking system that bakes collision data from SpriteShape;
2. Centralized data storage of all tiles in the game in a custom *database*;
3. Unfinished Sonic physics with unlocked refresh rate and parallelism in ECS;
4. Parallel tile collision in ECS;
5. Game menu with *Allocation Free* UI on UIToolkit;
6. ViewBinding source generator for UIToolkit;
7. Fluent (Step) Builder for ecs baking - BakerQuery;
8. Inverse kinematics, editable in the editor;
9. MonoBehaviour-signleton loader that creates them with DontDestroyOnLoad when starting any scene (it works both in the editor and in the build).

Development was frozen because I assessed the risks associated with developing a large project on Unity and decided to abandon it in favor of Godot.
