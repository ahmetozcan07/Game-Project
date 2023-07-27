using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BoarAI : MonoBehaviour
{
    Vector3[] patrolPoints = new Vector3[5]; // array of patrol points
    [SerializeField] private float sightDistance = 12f;
    [SerializeField] private float sensDistance = 2f;
    public LayerMask playerLayer;
    private float runningDistance = 5f;
    private Transform playerLastSeenAt;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float damage;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private HealthPoints healthPoints;
    private int currentPatrolIndex = 0;
    private bool fleeing = false;
    private bool waiting = false;
    private bool withinAttackRange = false;
    private bool readyForAttack = true;
    private GameObject player;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthPoints = GetComponent<HealthPoints>();
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] = new Vector3(0, 0, 0);
        }
        RandomPatrolPoints();

        // Walk to one of the patrol points
        if (patrolPoints.Length > 0)
        {
            navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex]);
        }

        Observable.EveryUpdate()
            .Subscribe(_ => ManageAnimations());
        Observable.EveryUpdate()
            .Subscribe(_ => PlayerSeen());
    }

    private void PlayerSeen()
    {
        Vector3 boarPosition = transform.position;
        if (!fleeing)
        {
            Patrol();
        }
        Vector3 rayDirection = transform.forward;
        RaycastHit hit;
        Collider[] colliders = Physics.OverlapSphere(transform.position, sensDistance, playerLayer);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (colliders.Length > 0)
        {
            playerLastSeenAt = colliders[0].transform;
            if (healthPoints.health < 40)
            {
                Flee(boarPosition);
            }
            else if (withinAttackRange)
            {
                if (readyForAttack)
                {
                    Attack();
                    Debug.Log("saldýrýyor: -=================================================> ");
                }
            }
            else if (!withinAttackRange)
            {
                Chase(playerLastSeenAt.position);
            }
            ManageAnimations();
        }
        else if (Physics.Raycast(boarPosition, rayDirection, out hit, sightDistance, playerLayer))
        {
            playerLastSeenAt = hit.transform;
            if(healthPoints.health < 40)
            {
                Flee(boarPosition);
            }
            else if (withinAttackRange)
            {
                if (readyForAttack)
                {
                    Attack();
                    Debug.Log("saldýrýyor: -=================================================> ");
                }
            }
            else if (!withinAttackRange)
            {
                Chase(playerLastSeenAt.position);
            }
            ManageAnimations();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (fleeing && Vector3.Distance(transform.position, playerLastSeenAt.position) > 50f) // stop running if distance is high
        {
            //start patrolling again
            RandomPatrolPoints();
            transform.rotation = Quaternion.Euler(0,
                Quaternion.LookRotation(playerLastSeenAt.position).eulerAngles.y, 0);
            fleeing = false;
        }
    }

    private void Patrol()
    {
        // Walk to next patrol point when arrived at one of patrol points
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f && !waiting && !fleeing)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            StartCoroutine(IdleAtPatrolPoint());
        }
    }

    private void Flee(Vector3 boarPosition)
    {
        Vector3 fleeDirection = transform.position - playerLastSeenAt.position;
        fleeDirection.y = 0f;
        Vector3 fleePosition = boarPosition + fleeDirection.normalized * runningDistance;
        navMeshAgent.SetDestination(fleePosition);
    }

    private void Chase(Vector3 playerPosition)
    {
        Debug.Log("kovalýyor");
        navMeshAgent.SetDestination(playerPosition);
        ManageAnimations();
    }
    private void Attack()
    {
        //for(int i = 0; i<3; i++)
        //{
        //    if (animator.GetBool("Attack" + i.ToString()))
        //    {
        //        animator.SetBool("Attack" + i.ToString(), false);
        //    }
        //}
        //string attack = "Attack" + Random.Range(0, 3).ToString();
        //animator.SetBool(attack, true);
        readyForAttack = false;
        player.GetComponent<PlayerStats>().TakeDamage(damage);
        StartCoroutine(WaitForAttackCooldown());
    }

    IEnumerator WaitForAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        readyForAttack = true;
    }


    IEnumerator IdleAtPatrolPoint()
    {
        waiting = true;
        yield return new WaitForSeconds(Random.Range(6, 13));
        waiting = false;
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex]);
    }

    private void RandomPatrolPoints()
    {
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] = new Vector3(transform.position.x + (Random.Range(0, 2) * 2 - 1) * Random.Range(6, 32), 0,
                transform.position.z + (Random.Range(0, 2) * 2 - 1) * Random.Range(6, 32));
        }
    }

    private void ManageAnimations()
    {
        if (waiting)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("Idle", true);
        }
        else if (fleeing)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("Idle", false);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("Idle", false);
            animator.SetBool("isWalking", true);
        }
    }
}