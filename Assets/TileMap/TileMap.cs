using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
	// TODO: Explore other data storage options
	[SerializeField] TextAsset mapDataText;

	int[][] mapData;
	public int TilesWide { get; private set; }
	public int TilesHigh { get; private set; }
	public int NumTiles { get; private set; }


	// TODO: Make sure MapData is square/not empty
	void Awake()
	{
		mapData = ParseTextToMapData(mapDataText);
		TilesWide = mapData[0].Length;
		TilesHigh = mapData.Length;
		NumTiles = TilesWide * TilesHigh;
	}


	// Retrieves tile at the specified (x,y) coordinate
	// Returns -1 if invalid indicies
	// TODO: Add a tile data structure
	// TODO: Proper exception handling
	public int GetTile(int x, int y)
	{
		if (x < 0 || x >= TilesWide || y < 0 || y >= TilesHigh)
		{
			return -1;
		}

		return mapData[y][x];
	}

	// Decrypts CSV map data file into a 2D int array of tile data
	// TODO: Tilemap is flipped vertically from text file
	int[][] ParseTextToMapData(TextAsset mapDataText)
	{
		// Split each line to a string[]
		string[] lines = mapDataText.text.Split('\n');

		// Initialize number of rows in mapData
		int[][] mapData = new int[lines.Length][];


		// Parse each line into an int[]
		for (int i = 0; i < lines.Length; i++)
		{
			// Split each entry from current line to a string
			string[] entriesString = lines[i].Split(',');

			// Initialize int[] to store converted strings
			int[] entries = new int[entriesString.Length];

			// Convert each entry to an int and store in mapData
			for (int j = 0; j < entriesString.Length; j++)
			{
				int.TryParse(entriesString[j], out entries[j]);
				mapData[i] = entries;
			}
		}
		return mapData;
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
