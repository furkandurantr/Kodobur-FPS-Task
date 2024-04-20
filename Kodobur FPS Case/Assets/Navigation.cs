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
    public float attackTime = 1f;
    public float attackRange = 5f;
    public float damage = 5f;
    public float FOV = 60f;
    public float LOS = 100f;
    int curPoint = 0;

    public enum EnemyState
    {
        idle,
        patrol,
        run,
        attack,
        attackRegain,
    }

    private IEnumerator idleCoroutine;
    Navigation.EnemyState myState;
    bool attackable = true;

    public Animator myAnim;
    // Start is called before the first frame update
    void Start()
    {
        myState = EnemyState.patrol;
        agent = GetComponent<NavMeshAgent>();
        //myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForSight();
        switch (myState)
        {
            case EnemyState.idle:
            myAnim.SetBool("IsMoving", false);
            myAnim.SetBool("IsRunning", false);
            myAnim.SetBool("IsAttacking", false);
            //myAnim.SetBool("IsAttacking", false);
                break;
            case EnemyState.patrol:
            myAnim.SetBool("IsMoving", true);
            myAnim.SetBool("IsRunning", false);
            Patrol();
                break;
            case EnemyState.run:
            myAnim.SetBool("IsRunning", true);
            myAnim.SetBool("IsMoving", false);
            Run();
                break;
            case EnemyState.attack:
                break;
            default:
            break;
        }

        //Debug.Log(myState);
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
            else if (myState == EnemyState.run || myState == EnemyState.attack)
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
            myAnim.SetBool("IsMoving", false);
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

    void Attack()
    {
        if (attackable == true)
        {
            //attackable = false;
            //idleCoroutine = Attacking();
            //StartCoroutine(idleCoroutine);
        }
        AttackAnim();
    }

    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(attackTime);
        float distance = Vector3.Distance (agent.transform.position, player.position);
        if (distance <= attackRange * 2 && myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            var target = player.gameObject.GetComponent<PlayerHP>();
            target.TakeDamage(damage);
        }
        attackable = true;
        myState = EnemyState.run;
    }

    void AttackAnim()
    {
        float distance = Vector3.Distance(agent.transform.position, player.position);
        if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f && !myAnim.IsInTransition(0))
        {
            if (distance <= attackRange * 2)
            {
                var target = player.gameObject.GetComponent<PlayerHP>();
                target.TakeDamage(damage);
                Debug.Log("Dealt Damage");
            }
            attackable = false;
            myState = EnemyState.idle;
        }
    }

    void Run()
    {
        agent.destination = player.position;
        float distance = Vector3.Distance (agent.transform.position, player.position);
        if (distance <= attackRange)
        {
            Attack();
            agent.destination = agent.transform.position;
            myAnim.SetBool("IsAttacking", true);
            attackable = true;
            myState = EnemyState.attack;
        }
        else
        {
            myAnim.SetBool("IsAttacking", false);
        }
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

    public void Debugla()
    {
        Debug.Log("Animation");
    }
}
