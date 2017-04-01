using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMapGFX))]
public class TileMapInspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("Refresh"))
		{
			TileMapGFX tileMap = (TileMapGFX)target;
			tileMap.InspectorRefresh();
		}
	}
}
