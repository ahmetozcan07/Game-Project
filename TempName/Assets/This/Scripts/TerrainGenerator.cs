using Unity.AI.Navigation;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 400; //x-axis of the terrain
    public int height = 400; //z-axis
    public int depth = 10; //y-axis
    public float scale = 10f;

    public float offsetX;
    public float offsetY;

    private NavMeshSurface[] surfaces;
    [SerializeField] private GameObject NavMesh;

    private void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);


   
        EnvironmentSpawner environmentSpawner = GetComponent<EnvironmentSpawner>();
        environmentSpawner.SpawnObjects();
           

        surfaces = NavMesh.GetComponentsInChildren<NavMeshSurface>();
        foreach (var surface in surfaces)
        {
            surface.BuildNavMesh();
        }

    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }
    
    
    float[,] GenerateHeights()
    {
        float[,] heights = new float[width + 1, height + 1];
        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }
    

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}