using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnController : MonoBehaviour
{
    public int maxEnemy = 5;
    public int maxPatrolNumber = 3;
    public GameObject enemy;
    public Transform player;
    public Transform myCamera;
    public Transform[] spawnPoints;
    public Transform[] patrolPoints;

    public int[] points = {3, 4, 5, 6, 7, 8, 9};
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
        
    }

    void SpawnEnemy()
    {
        GameObject curEnemy = Instantiate(enemy);
        int spawnRandom = Random.Range(0, spawnPoints.Length - 1);
        curEnemy.transform.position = spawnPoints[spawnRandom].position;

        Navigation curNav = curEnemy.GetComponent<Navigation>();
        Target curTarget = curEnemy.GetComponent<Target>();

        curNav.player = player;
        curTarget.camPos = myCamera;

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
    }
}
