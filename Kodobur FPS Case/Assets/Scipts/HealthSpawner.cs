using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    public GameObject healthSpawn;
    public int maxHealtSpawn = 3;
    public int curHealth = 0;
    float healthTimer = 0f;
    public float healthSpawnTime = 3f;
    public float spawnCollision = 1f;
    public Transform myCamera;
    public float randomX = 100f;
    public float randomZ = 100f;
    public LayerMask spawnLayer;
    // Start is called before the first frame update
    void Start()
    {
        for(int s = 0; s < maxHealtSpawn; s += 1)
        {
            SpawnHealth();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForHealthSpawns();
    }

    void CheckForHealthSpawns()
    {
        if (curHealth < maxHealtSpawn)
        {
            healthTimer += Time.deltaTime;
            if(healthTimer >= healthSpawnTime)
            {
                healthTimer = 0;
                SpawnHealth();
            }
        }
    }

    void SpawnHealth()
    {
        curHealth += 1;
        float randX = Random.Range(-randomX, randomX);
        float randZ = Random.Range(-randomZ, randomZ);
        Vector3 curPos = new Vector3(randX, healthSpawn.transform.position.y, randZ);
        while(Physics.CheckSphere(curPos, spawnCollision, spawnLayer))
        {
            randX = Random.Range(-randomX, randomX);
            randZ = Random.Range(-randomZ, randomZ);
            curPos = new Vector3(randX, healthSpawn.transform.position.y, randZ);
        }
        GameObject myObj = Instantiate(healthSpawn);
        var healthLoot = myObj.GetComponent<HealthLoot>();
        healthLoot.spawner = gameObject;
        healthLoot.camPos = myCamera;
        myObj.transform.position += curPos;
    }
}
