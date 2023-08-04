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


    private void Start()
    {
        InvokeRepeating("Spawn6", 0f, 360f);
        InvokeRepeating("Spawn3", 0f, 180f);
        InvokeRepeating("Spawn2", 0f, 120f);
    }


    private void Spawn6()
    {
        SpawnHunter();
    }

    private void Spawn3()
    {
        SpawnBoar();
        SpawnDeer();
        SpawnRabbit();
    }

    private void Spawn2()
    {
        SpawnMushroom();
        SpawnGrass();
    }



    Vector3 GetRandomObjectPosition()
    {
  
        GameObject terrainObject = GameObject.FindWithTag("Terrain");
        Terrain terrainComponent = terrainObject.GetComponent<Terrain>();
        TerrainData terrainData = terrainComponent.terrainData;



        Vector3 randomPosition = new Vector3(Random.Range(0f, terrainData.size.x), 0f, Random.Range(0f, terrainData.size.z));
        randomPosition.y = terrainComponent.SampleHeight(randomPosition);
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
        if (hunter < 1){

            Spawn(prefabs[0]);
        }
    }

    void SpawnBoar()
    {
        if (boar < 4){

            for (int i = 0; i < 2; i++)
            {
                Spawn(prefabs[1]);
                Spawn(prefabs[2]);
            }
        }
    }

    void SpawnDeer()
    {
        if (deer < 4){
            for (int i = 0; i < 2; i++)
            {
                Spawn(prefabs[3]);
                Spawn(prefabs[4]);
            }
        }
    }


    void SpawnRabbit()
    {
        if(rabbit < 8){
            Spawn(prefabs[5]);
            Spawn(prefabs[6]);
            Spawn(prefabs[7]);
            Spawn(prefabs[8]);
            Spawn(prefabs[9]);
            Spawn(prefabs[10]);
        }
    }

    void SpawnMushroom()
    {

        if (mushroom < 32){

            for (int i = 0; i < 2; i++)
            {
                Spawn(prefabs[11]);
                Spawn(prefabs[12]);
                Spawn(prefabs[13]);
                Spawn(prefabs[14]);
            }
        }
    }

    void SpawnGrass()
    {

        if (grass < 64){

            for (int i = 0; i < 4; i++)
            {
                Spawn(prefabs[15]);
                Spawn(prefabs[16]);
            }
            
        }
    }

     
                         

}








