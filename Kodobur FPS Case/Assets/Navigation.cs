using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    public Transform player;
    public GameObject point;
    public Transform[] patrolPoints;
    public NavMeshAgent agent;

    public float idleTime = 5f;
    public float FOV = 60f;
    public float LOS = 100f;
    int curPoint = 0;

    public enum EnemyState
    {
        idle,
        patrol,
        run,
        attack,
    }

    private IEnumerator idleCoroutine;
    Navigation.EnemyState myState;
    // Start is called before the first frame update
    void Start()
    {
        myState = EnemyState.patrol;
        agent = GetComponent<NavMeshAgent>();
        //IdleWait();
        //Will not randomly generate points
        //AddPoint(4);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForSight();
        switch (myState)
        {
            case EnemyState.idle:
                break;
            case EnemyState.patrol:
            Patrol();
                break;
            case EnemyState.run:
            Run();
                break;
            default:
            break;
        }
    }

    void CheckForSight()
    {
        RaycastHit[] hits;
        Vector3 myPos = transform.position;
        Vector3 myDir = transform.forward;
        
        Vector3 dir = (player.transform.position - myPos).normalized;
        //Debug.DrawLine(myPos, myPos + dir * 10, Color.red);
        hits = Physics.RaycastAll(transform.position, dir, LOS);

        System.Array.Sort(hits, (a, b) => (a.distance.CompareTo(b.distance)));

        float angle = Vector3.Angle(dir, transform.forward);

        angle = MathF.Abs(angle);
        if (hits.Length > 0)
        {
            if (hits[0].transform.gameObject == player.gameObject && angle <= FOV)
            {
                myState = EnemyState.run;
            }
            else if (myState == EnemyState.run)
            {
                idleCoroutine = IdleWait();
                StartCoroutine(idleCoroutine);
            }
        }
    }

    IEnumerator IdleWait()
    {
        if (myState == EnemyState.run)
        {
            yield break;
        }
        myState = EnemyState.idle;
        yield return new WaitForSeconds(idleTime);
        if (myState == EnemyState.idle)
        {
            myState = EnemyState.patrol;
        }
    }
    IEnumerator DamagedCooldown(float damagedTimer)
    {
        myState = EnemyState.run;
        yield return new WaitForSeconds(damagedTimer);
        if (myState == EnemyState.run)
        {
            myState = EnemyState.idle;
            idleCoroutine = IdleWait();
            StartCoroutine(idleCoroutine);
        }
    }

    void Patrol()
    {        
        agent.destination = patrolPoints[curPoint].position;
        float distance = Vector3.Distance (agent.transform.position, patrolPoints[curPoint].position);
        if (distance <= 1f)
        {    
            idleCoroutine = IdleWait();
            StartCoroutine(idleCoroutine);
            curPoint += 1;
            if (curPoint >= patrolPoints.Length)
            {
                curPoint = 0;
            }
        }
    }

    void Run()
    {
        agent.destination = player.position;
    }

    public void Damaged()
    {

        idleCoroutine = DamagedCooldown(5f);
        StartCoroutine(idleCoroutine);
    }

    void AddPoint(int number)
    {
        //patrolPoints = new Transform[number];
        
        //for (int i = 0; i < number; i += 1)
        //{
        //    Vector3 pos = new Vector3(Random.Range(-150f, 150f), 2.2f, Random.Range(-150f, 150f));
        //    GameObject curPoint = Instantiate(point);
        //    curPoint.transform.position = pos;
        //    patrolPoints[i] = curPoint.transform;
        //}
    }
}
