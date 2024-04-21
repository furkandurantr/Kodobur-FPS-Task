using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    public GameObject ammoSpawn;
    public int maxAmmoSpawn = 3;
    public int curAmmo = 0;
    float ammoTimer = 0f;
    public float ammoSpawnTime = 3f;
    public float spawnCollision = 1f;
    public Transform myCamera;
    public float randomX = 100f;
    public float randomZ = 100f;
    public LayerMask spawnLayer;
    // Start is called before the first frame update
    void Start()
    {
        for(int s = 0; s < maxAmmoSpawn; s += 1)
        {
            SpawnAmmo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForAmmoSpawns();
    }

    void CheckForAmmoSpawns()
    {
        if (curAmmo < maxAmmoSpawn)
        {
            ammoTimer += Time.deltaTime;
            if(ammoTimer >= ammoSpawnTime)
            {
                ammoTimer = 0;
                SpawnAmmo();
            }
        }
    }

    void SpawnAmmo()
    {
        curAmmo += 1;
        float randX = Random.Range(-randomX, randomX);
        float randZ = Random.Range(-randomZ, randomZ);
        Vector3 curPos = new Vector3(randX, ammoSpawn.transform.position.y, randZ);
        while(Physics.CheckSphere(curPos, spawnCollision, spawnLayer))
        {
            randX = Random.Range(-randomX, randomX);
            randZ = Random.Range(-randomZ, randomZ);
            curPos = new Vector3(randX, 0, randZ);
        }
        GameObject myObj = Instantiate(ammoSpawn);
        var ammoLoot = myObj.GetComponent<AmmoLoot>();
        ammoLoot.spawner = gameObject;
        ammoLoot.camPos = myCamera;
        myObj.transform.position += curPos;
    }
}
