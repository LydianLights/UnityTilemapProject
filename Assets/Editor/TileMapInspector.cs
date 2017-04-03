using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMapMeshBuilder))]
public class TileMapInspector : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		// Adds a button to rebuild the tilemap
		if(GUILayout.Button("Refresh"))
		{
			TileMapMeshBuilder tileMap = (TileMapMeshBuilder)target;

			// Calls awake, and then start on the tilemap
			tileMap.InspectorRefreshAwake();
			tileMap.InspectorRefreshStart();
		}
	}
}
