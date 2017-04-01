using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet : MonoBehaviour
{
	[SerializeField] Texture2D tileSetTexture;
	[SerializeField] int _tilesWide;
	// TODO: [SerializeField] int tileBuffer;

	public int TileResolution { get; private set; }
	public int TilesWide { get {return _tilesWide; } private set {_tilesWide = value;} }
	public int TilesHigh { get; private set; }
	public int NumTiles { get; private set; }


	void Awake()
	{
		// Initialize properties
		TileResolution = tileSetTexture.width / TilesWide;
		TilesHigh = tileSetTexture.height / TileResolution;
		NumTiles = TilesWide * TilesHigh;

		// Make sure tileset texture divides evenly into tiles
		// TODO: Improve error handling
		if(tileSetTexture.width % TilesWide != 0 || tileSetTexture.height % TilesHigh != 0)
		{
			Debug.LogWarning("Tileset texture not divided evenly into tiles! Check 'Tiles Wide' property in inspector.");
		}
	}

	// Returns a Color[] corresponding to the requested tile
	// Returns null on invalid index
	public Color[] GetTilePixels(int tileIndex)
	{
		// TODO: Proper exception handling
		if (tileIndex < 0 || tileIndex >= NumTiles)
		{
			return null;
		}

		int x = tileIndex % TilesWide;
		int y = tileIndex / TilesWide;

		return tileSetTexture.GetPixels(x * TileResolution, y * TileResolution, TileResolution, TileResolution);
	}


	// Public functions for inspector mode
	public void InspectorRefresh()
	{
		Awake();
	}
}
