

public class TileMapData
{
	TileData[] tiles;
	int width;
	int height;


	public TileMapData(int width, int height)
	{
		this.width = width;
		this.height = height;

		tiles = new TileData[width * height];
	}

	// Returns the tile at the specified (x,y) coordinate
	// Returns null if tile is outside of range
	public TileData GetTile(int x, int y)
	{
		// TODO: Proper try/catch handling
		if (x < 0 || x >= width || y < 0 || y >= height)
		{
			return null;
		}
		
		return tiles[y * width + x];
	}
}
