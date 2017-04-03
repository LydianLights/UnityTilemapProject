using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(TileMap))]
public class TileMapMeshBuilder : MonoBehaviour
{
	// The tilemap to build the mesh for
	TileMap map;

	// Tileset to load tiles from
	[SerializeField] TileSet tileSet;

	// Object mesh components
	MeshFilter meshFilter;
	MeshRenderer meshRenderer;
	MeshCollider meshCollider;

	// Unity scale of the tilemap
	[SerializeField] float _tileScale = 1.0f;
	public float TileScale {get { return _tileScale; } private set { _tileScale = value; } }


	void Awake()
	{
		// Get reference to game object components
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
		map = GetComponent<TileMap>();
	}

	void Start()
	{
		// Build mesh and texture it
		BuildMesh();
	}


	// Generates mesh data for the tilemap
	// Mesh built such that each tile = one quad in the mesh
	void BuildMesh()
	{
		int numVertices = 4 * map.NumTiles;
		int numTriangles = 2 * map.NumTiles * 3;

		// Initialize arrays for mesh data
		Vector3[] vertices = new Vector3[numVertices];
		int[] triangles = new int[numTriangles * 3];
		Vector3[] normals = new Vector3[numVertices];
		Vector2[] uv = new Vector2[numVertices];


		// Iterate through each tile and set verticies/triangles/uvs
		for(int i = 0; i < map.NumTiles; i++)
		{
			int currentTileIndex = map.GetTileByIndex(i);

			int vertOffset = 4 * i;
			int triOffset = 6 * i;


			// Set vertex coords of current tile
			int xOffset = i % map.TilesWide;
			int zOffset =  i / map.TilesWide;

			float y = 0;
			float x1 = xOffset * TileScale;
			float x2 = (xOffset + 1) * TileScale;
			float z1 = zOffset * TileScale;
			float z2 = (zOffset + 1) * TileScale;

			vertices[vertOffset + 0] = new Vector3(x1, y, z1);
			vertices[vertOffset + 1] = new Vector3(x2, y, z1);
			vertices[vertOffset + 2] = new Vector3(x1, y, z2);
			vertices[vertOffset + 3] = new Vector3(x2, y, z2);


			// Set the triangles of current tile
			// Triangle 1
			triangles[triOffset + 0] = vertOffset + 0;
			triangles[triOffset + 1] = vertOffset + 2;
			triangles[triOffset + 2] = vertOffset + 1;

			// Triangle 2
			triangles[triOffset + 3] = vertOffset + 1;
			triangles[triOffset + 4] = vertOffset + 2;
			triangles[triOffset + 5] = vertOffset + 3;


			// Set up uvs for current tile
			// Gets pixel coordinates of tile's bottom-left corner in tileset texture
			Vector2 tileOffset = tileSet.GetTileTextureOffset(currentTileIndex);

			// Find each corner of tile as a percentage of full texture size
			float uv_x1 = tileOffset.x / tileSet.Texture.width;
			float uv_x2 = (tileOffset.x + tileSet.TileResolution) / tileSet.Texture.width;
			float uv_y1 = tileOffset.y / tileSet.Texture.height;
			float uv_y2 = (tileOffset.y + tileSet.TileResolution) / tileSet.Texture.height;


			uv[vertOffset + 0] = new Vector2(uv_x1, uv_y1);
			uv[vertOffset + 1] = new Vector2(uv_x2, uv_y1);
			uv[vertOffset + 2] = new Vector2(uv_x1, uv_y2);
			uv[vertOffset + 3] = new Vector2(uv_x2, uv_y2);
		}


		// Set normal vector for each vertex
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


		meshFilter.mesh = mesh;
		meshRenderer.sharedMaterial.mainTexture = tileSet.Texture;
	}


	// Public functions for inspector mode
	#if UNITY_EDITOR
	public void InspectorRefreshAwake()
	{
		Awake();
		tileSet.InspectorRefreshAwake();
		map.InspectorRefreshAwake();
	}

	public void InspectorRefreshStart()
	{
		Start();
		tileSet.InspectorRefreshStart();
		map.InspectorRefreshStart();
	}
	#endif
}