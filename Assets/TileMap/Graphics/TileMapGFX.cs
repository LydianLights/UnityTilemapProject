using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(TileMapData))]
public class TileMapGFX : MonoBehaviour
{
	// Tileset to load tiles from
	[SerializeField] TileSet tileSet;

	// Data describing the tilemap
	TileMapData data;

	// Unity scale of the tilemap
	public float tileScale = 1.0f;


	void Awake()
	{
		data = GetComponent<TileMapData>();
	}

	void Start()
	{
		// Build mesh and texture it
		BuildMesh();
	}


	// Generates mesh data for the tilemap
	// Mesh built such that each tile = one square in the mesh
	// TODO: Re-evalutate mesh construction -- try one quad per tile for efficient mapping of tile texture
	void BuildMesh()
	{
		// DEBUG: Hardcoded for testing
		int numVertices = 8;
		int numTriangles = 4;
		int numTiles = 2;

		// Initialize arrays for mesh data
		Vector3[] vertices = new Vector3[numVertices];
		int[] triangles = new int[numTriangles * 3];
		Vector3[] normals = new Vector3[numVertices];
		Vector2[] uv = new Vector2[numVertices];


		// DEBUG: Hardcoded for testing
		int dbg_offset = 2;
		vertices[0] = new Vector3(0, 0, 0);
		vertices[1] = new Vector3(1, 0, 0);
		vertices[2] = new Vector3(0, 0, 1);
		vertices[3] = new Vector3(1, 0, 1);

		vertices[4] = new Vector3(0 + dbg_offset, 0, 0);
		vertices[5] = new Vector3(1 + dbg_offset, 0, 0);
		vertices[6] = new Vector3(0 + dbg_offset, 0, 1);
		vertices[7] = new Vector3(1 + dbg_offset, 0, 1);


		// DEBUG: Hardcoded for testing
		triangles[0] = 0;
		triangles[1] = 2;
		triangles[2] = 1;

		triangles[3] = 1;
		triangles[4] = 2;
		triangles[5] = 3;

		triangles[6] = 4;
		triangles[7] = 6;
		triangles[8] = 5;

		triangles[9] = 5;
		triangles[10] = 6;
		triangles[11] = 7;


		// DEBUG: Hardcoded for testing
		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2((float)tileSet.TileResolution / tileSet.Texture.width, 0);
		uv[2] = new Vector2(0, (float)tileSet.TileResolution / tileSet.Texture.height);
		uv[3] = new Vector2((float)tileSet.TileResolution / tileSet.Texture.width, (float)tileSet.TileResolution / tileSet.Texture.height);

		uv[4] = new Vector2((float)tileSet.TileResolution / tileSet.Texture.width, (float)tileSet.TileResolution / tileSet.Texture.height);
		uv[5] = new Vector2(2 * (float)tileSet.TileResolution / tileSet.Texture.width, (float)tileSet.TileResolution / tileSet.Texture.height);
		uv[6] = new Vector2((float)tileSet.TileResolution / tileSet.Texture.width, 2 * (float)tileSet.TileResolution / tileSet.Texture.height);
		uv[7] = new Vector2(2 * (float)tileSet.TileResolution / tileSet.Texture.width, 2 * (float)tileSet.TileResolution / tileSet.Texture.height);


		for (int i = 0; i < numVertices; i++)
		{
			normals[i] = Vector3.up;
		}










		// Create new mesh and populate with mesh data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;


		// Assign mesh to filter/renderer/collider
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		MeshCollider meshCollider = GetComponent<MeshCollider>();

		meshFilter.mesh = mesh;
		meshRenderer.sharedMaterial.mainTexture = tileSet.Texture;
	}


	// Public functions for inspector mode
	#if UNITY_EDITOR
	public void InspectorRefreshAwake()
	{
		Awake();
		tileSet.InspectorRefreshAwake();
		data.InspectorRefreshAwake();
	}

	public void InspectorRefreshStart()
	{
		Start();
		tileSet.InspectorRefreshStart();
		data.InspectorRefreshStart();
	}
	#endif
}