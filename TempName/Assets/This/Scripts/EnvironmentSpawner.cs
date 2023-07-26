using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    private int[] numberOfPrefabsToSpawn = {
        10, 10, 10, 10, 10, 10, 10, //tree 100
        10, 10,//bush 20
        100, 100, //grass 250
        10, 10, 10, 10 //rock 40
    };
    private float minimumSpacing = 2; //minimum mesafe


    public void SpawnObjects()
    {
        TerrainData terrainData = GetComponent<Terrain>().terrainData;

        for (int i = 0; i < prefabs.Length; i++)
        {
            for (int j = 0; j < numberOfPrefabsToSpawn[i]; j++)
            {
                Vector3 randomPosition = GetRandomObjectPosition(terrainData);
                if (IsPositionValid(randomPosition))
                {
                    //GameObject newObject = Instantiate(prefabs[i], randomPosition, Quaternion.identity);
                    GameObject newObject = Instantiate(prefabs[i], randomPosition, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                }
            }
        }
    }

    Vector3 GetRandomObjectPosition(TerrainData terrainData)
    {
        Vector3 randomPosition = new Vector3(Random.Range(0f, terrainData.size.x), 0f, Random.Range(0f, terrainData.size.z));
        randomPosition.y = GetComponent<Terrain>().SampleHeight(randomPosition);
        return randomPosition;
    }

    bool IsPositionValid(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, minimumSpacing);
        foreach (Collider collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Terrain"))
            {
                return false;
            }
        }
        return true;
    }
}








