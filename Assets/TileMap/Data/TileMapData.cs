using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapData : MonoBehaviour
{
	// TODO: Explore other data storage options
	[SerializeField] TextAsset mapDataText;
	int[][] mapData;


	void Awake()
	{
		mapData = ParseTextToMapData(mapDataText);
	}


	// Decrypts CSV map data file into a 2D int array of tile data
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
	public void InspectorRefresh()
	{
		Awake();
	}
}
