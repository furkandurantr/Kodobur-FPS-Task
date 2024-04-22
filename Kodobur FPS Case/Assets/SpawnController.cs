using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnController : MonoBehaviour
{
    public int maxEnemy = 5;
    public int maxPatrolNumber = 3;
    public GameObject[] enemy;
    public Transform player;
    public Transform myCamera;
    public float enemySpawnTime = 10f;
    float spawnerTime = 0f;
    public Transform[] spawnPoints;
    public Transform[] patrolPoints;
    int currentEnemyNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int s = 0; s < maxEnemy; s += 1)
        {
            SpawnEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForSpawns();
    }

    void CheckForSpawns()
    {
        if (currentEnemyNumber < maxEnemy + (player.GetComponent<PlayerLevel>().curLevel - 1))
        {
            spawnerTime += Time.deltaTime;
            if (spawnerTime >= enemySpawnTime)
            {
                SpawnEnemy();
            }
        }
    }

    public void EnemyDead()
    {
        currentEnemyNumber -= 1;
    }

    void SpawnEnemy()
    {
        int randEnemy = Random.Range(0, enemy.Length);
        GameObject curEnemy = Instantiate(enemy[randEnemy]);
        int spawnRandom = Random.Range(0, spawnPoints.Length - 1);
        curEnemy.transform.position = spawnPoints[spawnRandom].position;

        Navigation curNav = curEnemy.GetComponent<Navigation>();
        Target curTarget = curEnemy.GetComponent<Target>();

        curNav.player = player;
        curTarget.camPos = myCamera;
        curTarget.spawner = gameObject;

        int[] patrolIndices = new int[maxPatrolNumber];

        Transform[] tempCopy = new Transform[maxPatrolNumber];

        List<Transform> shuffledPoints = new List<Transform>(patrolPoints);
        curNav.patrolPoints = new Transform[maxPatrolNumber];
        
        for (int i = 0; i < maxPatrolNumber; i += 1)
        {
            Transform temp = shuffledPoints[i];
            int randomIndex = Random.Range(i, shuffledPoints.Count - 1);
            shuffledPoints[i] = shuffledPoints[randomIndex];
            curNav.patrolPoints[i] = shuffledPoints[randomIndex];
            shuffledPoints[randomIndex] = temp;
            
        }
        currentEnemyNumber += 1;
        spawnerTime = 0;
    }
}
