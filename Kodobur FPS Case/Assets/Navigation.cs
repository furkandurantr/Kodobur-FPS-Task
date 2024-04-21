using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;
    public NavMeshAgent agent;

    public float idleTime = 5f;
    public float aggroTime = 5f;
    public float attackTime = 1f;
    public float attackRange = 5f;
    public float damage = 5f;
    public float FOV = 60f;
    public float LOS = 100f;
    int curPoint = 0;

    float mustRun = 0f;
    float mustWait = 0f;

    public enum EnemyState
    {
        idle,
        idleEnd,
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
        switch (myState)
        {
            case EnemyState.idle:
            myAnim.SetBool("IsMoving", false);
            myAnim.SetBool("IsRunning", false);
            if (IsInSight())
            {
                myState = EnemyState.run;
            }
                break;
            case EnemyState.patrol:
            myAnim.SetBool("IsMoving", true);
            myAnim.SetBool("IsRunning", false);
            if (IsInSight())
            {
                myState = EnemyState.run;
            }
            Patrol();
                break;
            case EnemyState.run:
            myAnim.SetBool("IsRunning", true);
            myAnim.SetBool("IsMoving", false);
            Run();
                break;
            case EnemyState.attack:
            Attack();
                break;
            default:
            break;
        }
        MustRun();
        MustWait();
    }

    void MustWait()
    {
        if (mustWait > 0)
        {
            mustWait -= Time.deltaTime;
            agent.destination = transform.position;
            myAnim.SetBool("IsRunning", false);
            if (IsInSight())
            {
                mustWait = 0f;
            }
        }
        else if (myState == EnemyState.idle)
        {
            myState = EnemyState.patrol;
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
    void MustRun()
    {
        if (mustRun > 0)
        {
            mustWait = 0f;
            mustRun -= Time.deltaTime;
            myState = EnemyState.run;
            if (myState == EnemyState.attack)
            {
                mustRun = 0f;
            }
        }
    }

    bool IsInSight()
    {
        bool seen = false;
        RaycastHit[] hits;
        Vector3 myPos = transform.position;
        Vector3 myDir = transform.forward;
        
        Vector3 dir = (player.transform.position - myPos).normalized;
        //This raycast can be changed with spherecast/capsulecast
        hits = Physics.RaycastAll(transform.position, dir, LOS);

        System.Array.Sort(hits, (a, b) => (a.distance.CompareTo(b.distance)));
        //Fake FOV
        float angle = Vector3.Angle(dir, transform.forward);

        angle = MathF.Abs(angle);
        if (hits.Length > 0)
        {
            if (hits[0].transform.gameObject == player.gameObject && angle <= FOV)
            {
                seen = true;
            }
        }
        return seen;
    }
    void Patrol()
    {        
        agent.destination = patrolPoints[curPoint].position;
        float distance = Vector3.Distance (agent.transform.position, patrolPoints[curPoint].position);
        if (distance <= 1f)
        {    
            mustWait = idleTime;
            myState = EnemyState.idle;
            curPoint += 1;
            if (curPoint >= patrolPoints.Length)
            {
                curPoint = 0;
            }
        }
    }

    void Attack()
    {
        AttackAnim();
    }

    void AttackAnim()
    {
        float distance = Vector3.Distance(agent.transform.position, player.position);
        if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f && !myAnim.IsInTransition(0) && attackable == true)
        {
            attackable = false;
            if (distance <= attackRange * 2)
            {
                var target = player.gameObject.GetComponent<PlayerHP>();
                target.TakeDamage(damage);
                Debug.Log("Dealt Damage");
                myAnim.SetBool("IsAttacking", false);
                myState = EnemyState.run;
            }
            else
            {
                myState = EnemyState.idle;
            }
        }
    }

    void Run()
    {
        agent.destination = player.position;
        float distance = Vector3.Distance (agent.transform.position, player.position);
        if (distance <= attackRange)
        {
            agent.destination = agent.transform.position;
            myAnim.SetBool("IsAttacking", true);
            myState = EnemyState.attack;
            attackable = true;
        }
        else
        {
            myAnim.SetBool("IsAttacking", false);
        }
        if (!IsInSight())
        {
            mustWait = idleTime;
            myState = EnemyState.idle;
        }
    }

    public void Damaged()
    {
        mustRun = aggroTime;
    }

    void ChooseState()
    {
        
    }

    public void Debugla()
    {
        Debug.Log("Animation");
    }
}
