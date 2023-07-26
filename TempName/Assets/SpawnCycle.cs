using UnityEngine;

public class SpawnCycle : MonoBehaviour
{
    public int hunter;
    public int boar;
    public int deer;
    public int rabbit;
    public int mushroom;
    public int grass;
    //tüketildiðinde bu scriptte azaltýlmalý

    public GameObject[] prefabs;
    //Hunter1 Boar1 Deer2 Rabbit1 Mushroom4 Grass2 = 11 prefab with this order
    private float minimumSpacing = 2; //minimum mesafe


    public void TestThenSpawn()
    {
        SpawnHunter();
        

  
    }

    Vector3 GetRandomObjectPosition()
    {
        TerrainData terrainData = GetComponent<Terrain>().terrainData;
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

    void Spawn (GameObject prefab)
    {
        Vector3 randomPosition = GetRandomObjectPosition();
        if (IsPositionValid(randomPosition))
        {
            GameObject newObject = Instantiate(prefab, randomPosition, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
        }
    }

    void SpawnHunter ()
    {
        if (hunter < 1){//limit

            Spawn(prefabs[0]);
        }
    }

    void SpawnBoar()
    {
        if (boar < 4){//limit

            Spawn(prefabs[1]);
        }
    }

    void SpawnDeer()
    {
        if (deer < 4){//limit

            Spawn(prefabs[2]);
            Spawn(prefabs[3]);

        }
    }


    void SpawnRabbit()
    {
        if(rabbit < 8){//limit
            Spawn(prefabs[4]);
        }
    }

    void SpawnMushroom()
    {

        if (mushroom < 32){//limit


            Spawn(prefabs[5]);
            Spawn(prefabs[6]);
            Spawn(prefabs[7]);
            Spawn(prefabs[8]);

        }
    }

    void SpawnGrass()
    {

        if (grass < 64){//limit

            Spawn(prefabs[9]);
            Spawn(prefabs[10]);
        }
    }

     
                         

}








