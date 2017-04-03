using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet : MonoBehaviour
{
	[SerializeField] Texture2D _texture;
	[SerializeField] int _tilesWide;
	// TODO: [SerializeField] int tileBuffer;

	public Texture2D Texture { get {return _texture; } private set {_texture = value;} }
	public int TileResolution { get; private set; }
	public int TilesWide { get {return _tilesWide; } private set {_tilesWide = value;} }
	public int TilesHigh { get; private set; }
	public int NumTiles { get; private set; }


	void Awake()
	{
		// Initialize properties
		TileResolution = Texture.width / TilesWide;
		TilesHigh = Texture.height / TileResolution;
		NumTiles = TilesWide * TilesHigh;

		// Make sure tileset texture divides evenly into tiles
		// TODO: Improve error handling
		if(Texture.width % TilesWide != 0 || Texture.height % TilesHigh != 0)
		{
			Debug.LogWarning("Error at " + this.name + ": Tileset texture not divided evenly into tiles! Check 'Tiles Wide' property in inspector.");
		}
	}


	// Returns the position of the bottom-left corner of the tile within the texture
	public Vector2 GetTileTextureOffset(int tileIndex)
	{
		int xOffset = tileIndex % TilesWide;
		int yOffset = TilesHigh - 1 - (tileIndex / TilesWide);

		return new Vector2(xOffset * TileResolution, yOffset * TileResolution);
	}


	// Public functions for inspector mode
	#if UNITY_EDITOR
	public void InspectorRefreshAwake()
	{
		Awake();
	}

	public void InspectorRefreshStart()
	{
		//Start();
	}
	#endif
}
