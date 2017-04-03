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
	[SerializeField] float tileScale = 1.0f;
	public float TileScale {get { return tileScale; } private set { tileScale = value; } }


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
	// Mesh built such that each tile = one square in the mesh
	// TODO: Re-evalutate mesh construction -- try one quad per tile for efficient mapping of tile texture
	void BuildMesh()
	{
		int numVertices = 4 * map.NumTiles;
		int numTriangles = 2 * 3 * map.NumTiles;

		// Initialize arrays for mesh data
		Vector3[] vertices = new Vector3[numVertices];
		int[] triangles = new int[numTriangles * 3];
		Vector3[] normals = new Vector3[numVertices];
		Vector2[] uv = new Vector2[numVertices];


		// Iterate through each tile and set verticies/triangles
		for(int i = 0; i < map.NumTiles; i++)
		{
			int vertOffset = 4 * i;
			int triOffset = 2 * 3 * i;
			int xOffset = i % map.TilesWide;
			int zOffset =  i / map.TilesWide;
			float yOffset = 0;


			// Set vertex coords of current tile
			vertices[vertOffset + 0] = new Vector3(		xOffset * TileScale,			yOffset,		zOffset * TileScale			);
			vertices[vertOffset + 1] = new Vector3(		(xOffset + 1) * TileScale,		yOffset,		zOffset * TileScale			);
			vertices[vertOffset + 2] = new Vector3(		xOffset * TileScale,			yOffset,		(zOffset + 1) * TileScale	);
			vertices[vertOffset + 3] = new Vector3(		(xOffset + 1) * TileScale,		yOffset,		(zOffset + 1) * TileScale	);


			// Set the triangles of current tile
			// Triangle 1
			triangles[triOffset + 0] = vertOffset + 0;
			triangles[triOffset + 1] = vertOffset + 2;
			triangles[triOffset + 2] = vertOffset + 1;

			// Triangle 2
			triangles[triOffset + 3] = vertOffset + 1;
			triangles[triOffset + 4] = vertOffset + 2;
			triangles[triOffset + 5] = vertOffset + 3;
		}


		// Set normal vector for each vertex
		for (int i = 0; i < numVertices; i++)
		{
			normals[i] = Vector3.up;
		}


		// DEBUG: Hardcoded for testing
		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2((float)tileSet.TileResolution / tileSet.Texture.width, 0);
		uv[2] = new Vector2(0, (float)tileSet.TileResolution / tileSet.Texture.height);
		uv[3] = new Vector2((float)tileSet.TileResolution / tileSet.Texture.width, (float)tileSet.TileResolution / tileSet.Texture.height);

		uv[4] = new Vector2((float)tileSet.TileResolution / tileSet.Texture.width, (float)tileSet.TileResolution / tileSet.Texture.height);
		uv[5] = new Vector2(2 * (float)tileSet.TileResolution / tileSet.Texture.width, (float)tileSet.TileResolution / tileSet.Texture.height);
		uv[6] = new Vector2((float)tileSet.TileResolution / tileSet.Texture.width, 2 * (float)tileSet.TileResolution / tileSet.Texture.height);
		uv[7] = new Vector2(2 * (float)tileSet.TileResolution / tileSet.Texture.width, 2 * (float)tileSet.TileResolution / tileSet.Texture.height);











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