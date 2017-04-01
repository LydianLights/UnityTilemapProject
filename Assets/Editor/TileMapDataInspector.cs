using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMapData))]
public class TileMapDataInspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("Refresh"))
		{
			TileMapData tileMapData = (TileMapData)target;
			tileMapData.InspectorRefresh();
		}
	}
}
