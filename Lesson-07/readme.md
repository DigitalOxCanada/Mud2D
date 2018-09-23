# Lesson Notes

1. Replaced usage of GetType() and typeof() with object boolean IsWalkable.

2. The Map constructor takes filename and now performs all loading/parsing inside the Map class.

3. Added Monster class and List of monsters to track in the Map.

4. Added a single monster at random location in blank space.

5. Player and monsters are now drawn after the map using Console.SetCursorLocation().

6. Added stub for Monster AI to be called.

7. Introduction to Linq `var blanks = (from t in Tiles.Cast<MapTile>() where t.GetType() == typeof(MapTileSpace) select t).ToArray();`.  Good resource (https://code.msdn.microsoft.com/101-LINQ-Samples-3fb9811b).