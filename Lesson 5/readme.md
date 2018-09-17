# Lesson Notes

1. Changed mapSettings into a Map class.

2. Created model classes for the map tiles/cells.

3. Utilized GetType() and typeof() to detect maptile object types.

4. Made all map coding object oriented using class inheritance.  Map tiles are specific map classes derived from the base MapTile class.  Map class is responsible for creating tiles and for drawing itself.
```c
public class MapTile {
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; set; }
}

public class MapTileWall : MapTile
{
    public MapTileWall()
    {
        Symbol = '#';
    }
}
```