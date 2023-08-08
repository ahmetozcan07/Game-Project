using UnityEngine;

public class SpawnCycle : MonoBehaviour
{
    public int hunterNumber;
    public int boarNumber;
    public int deerNumber;
    public int rabbitNumber;
    public int mushroomNumber;
    public int grassNumber;
    //tüketildiðinde bu scriptte azaltýlmalý

    private int hunterCreating = 1;
    private int boarCreating = 4;
    private int deerCreating = 4;
    private int rabbitCreating = 6;
    private int mushroomCreating = 8;
    private int grassCreating = 8;

    private int hunterRestrict = 1;
    private int boarRestrict = 4;
    private int deerRestrict = 4;
    private int rabbitRestrict = 8;
    private int mushroomRestrict = 32;
    private int grassRestrict = 64;


    public GameObject[] prefabs;
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
        if (hunterNumber < hunterRestrict){

            for (int i = 0; i < hunterCreating/1; i++)
            {
                Spawn(prefabs[0]);
            }
            hunterNumber += hunterCreating;
        }
    }

    void SpawnBoar()
    {
        if (boarNumber < boarRestrict){

            for (int i = 0; i < boarCreating/2; i++)
            {
                Spawn(prefabs[1]);
                Spawn(prefabs[2]);
            }
            boarNumber += boarCreating;
        }
    }

    void SpawnDeer()
    {
        if (deerNumber < deerRestrict){
            for (int i = 0; i < deerCreating/2; i++)
            {
                Spawn(prefabs[3]);
                Spawn(prefabs[4]);
            }
            deerNumber += deerCreating;
        }
    }


    void SpawnRabbit()
    {
        if(rabbitNumber < rabbitRestrict){
            for (int i = 0; i < rabbitCreating/6; i++)
            {
                Spawn(prefabs[5]);
                Spawn(prefabs[6]);
                Spawn(prefabs[7]);
                Spawn(prefabs[8]);
                Spawn(prefabs[9]);
                Spawn(prefabs[10]);
            }
            rabbitNumber += rabbitCreating;
        }
        
    }

    void SpawnMushroom()
    {

        if (mushroomNumber < mushroomRestrict){

            for (int i = 0; i < mushroomCreating/4; i++)
            {
                Spawn(prefabs[11]);
                Spawn(prefabs[12]);
                Spawn(prefabs[13]);
                Spawn(prefabs[14]);
            }
            mushroomNumber += mushroomCreating;
        }
    }

    void SpawnGrass()
    {

        if (grassNumber < grassRestrict){

            for (int i = 0; i < grassCreating/2; i++)
            {
                Spawn(prefabs[15]);
                Spawn(prefabs[16]);
            }
            grassNumber += grassCreating;
        }
    }

     
                         

}








