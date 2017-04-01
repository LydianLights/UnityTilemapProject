using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(TileMapData))]
public class TileMapGFX : MonoBehaviour
{
	// DEBUG
	public int dbg_TileNumber;

	// Tileset to load tiles from
	[SerializeField] TileSet tileSet;

	// Dimensions of tilemap in terms of tiles
	public int numTilesX;
	public int numTilesZ;
	int numTiles;

	// Unity scale of the tilemap
	public float tileScale = 1.0f;


	void Awake()
	{
		// Initialize tilemap data
		numTiles = numTilesX * numTilesZ;
	}

	void Start()
	{
		// Build mesh and texture
		BuildMesh();
		BuildTexture();
	}


	// Generates mesh data for the tilemap
	// Mesh built such that each tile = one square in the mesh
	// TODO: Re-evalutate mesh construction -- try one quad per tile for efficient mapping of tile texture
	void BuildMesh()
	{
		// Dimensions of mesh in terms of vertices
		int numVerticesX = numTilesX + 1;
		int numVerticesZ = numTilesZ + 1;
		int numVertices = numVerticesX * numVerticesZ;

		// Number of triangles in the mesh
		int numTriangles = 2 * numTiles;

		// Initialize arrays for mesh data
		Vector3[] vertices = new Vector3[numVertices];
		int[] triangles = new int[numTriangles * 3];
		Vector3[] normals = new Vector3[numVertices];
		Vector2[] uv = new Vector2[numVertices];


		// Generate verticies/normals/uvs
		for (int z = 0; z < numVerticesZ; z++)
		{
			for (int x = 0; x < numVerticesX; x++)
			{
				int currentVertex = z * numVerticesX + x;

				// Place vertex at coordinates (x, z) * tileScale
				vertices[currentVertex] = new Vector3(x * tileScale, 0, z * tileScale);
				normals[currentVertex] = Vector3.up;

				// Evenly map texture to mesh
				float uvX = (float)x / (float)(numVerticesX - 1);
				float uvZ = (float)z / (float)(numVerticesZ - 1);
				uv[currentVertex] = new Vector2(uvX, uvZ);
			}
		}

		// Generate triangles for each tile
		for (int z = 0; z < numTilesZ; z++)
		{
			for(int x = 0; x < numTilesX; x++)
			{
				int currentTile = z * numTilesX + x;
				int topLeftVertex = currentTile + z;

				// Triangle 1
				triangles[6 * currentTile + 0] = topLeftVertex + 0;
				triangles[6 * currentTile + 1] = topLeftVertex + 0 + numVerticesX;
				triangles[6 * currentTile + 2] = topLeftVertex + 1 + numVerticesX;

				// Triangle 2
				triangles[6 * currentTile + 3] = topLeftVertex + 0;
				triangles[6 * currentTile + 4] = topLeftVertex + 1 + numVerticesX;
				triangles[6 * currentTile + 5] = topLeftVertex + 1;
			}
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
	}


	// Procedurally generate texture from tileset
	// Builds new texture of entire tilemap and applies to mesh
	// TODO: Try making each tile a quad with proper uv mapping to avoid this step
	void BuildTexture()
	{
		// Determine size of each tile
		int tileResolution = tileSet.TileResolution;

		// Determine size of full texture
		int textureWidth = numTilesX * tileResolution;
		int textureHeight = numTilesZ * tileResolution;

		Texture2D texture = new Texture2D(textureWidth, textureHeight);

		for(int y = 0; y < numTilesZ; y++)
		{
			for(int x = 0; x < numTilesX; x++)
			{
				// TODO: Remove hardcoded tile number in favor of TileMapData
				Color[] p = tileSet.GetTilePixels(dbg_TileNumber);
				texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.Apply();

		// TODO: sharedMaterial vs material
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.sharedMaterial.mainTexture = texture;
	}


	// Public functions for inspector mode
	public void InspectorRefresh()
	{
		tileSet.InspectorRefresh();
		Awake();
		Start();
	}
}
