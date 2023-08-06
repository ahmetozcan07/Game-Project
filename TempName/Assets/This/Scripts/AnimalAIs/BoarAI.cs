using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class BoarAI : MonoBehaviour
{
    Vector3[] patrolPoints = new Vector3[5]; // array of patrol points
    [SerializeField] private float sensDistance;
    [SerializeField] private float attackRange;
    public LayerMask playerLayer;
    private float runningDistance = 5f;
    private Transform playerLastSeenAt;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage = 10;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private HealthPoints healthPoints;
    private int currentPatrolIndex = 0;
    private bool fleeing = false;
    private bool waiting = false;
    private bool chasing = false;
    private bool attacking = false;
    private bool canPatrol = true;
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
            .Subscribe(_ => ManageAnimations()).AddTo(this);
        Observable.EveryUpdate()
            .Subscribe(_ => AILifeCycle()).AddTo(this);
    }

    private void AILifeCycle()
    {
        if (healthPoints.isDead)
        {
            navMeshAgent.isStopped = true;
        }
        Vector3 boarPosition = transform.position;
        if (canPatrol)
        {
            Patrol();
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, sensDistance, playerLayer);
        if (colliders.Length > 0)
        {
            canPatrol = false;
            player = colliders[0].gameObject;
            playerLastSeenAt = player.transform;
            Collider[] collidersInAttackRange = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
            if (healthPoints.health < 40)
            {
                Flee(boarPosition);
            }
            else if (collidersInAttackRange.Length > 0)
            {
                chasing = false;
                Vector3 directionToTarget = player.transform.position - transform.position;
                directionToTarget.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = targetRotation;
                if (!attacking)
                {
                    Attack();
                }
            }
            else
            {
                Chase(playerLastSeenAt.position);
            }
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
    IEnumerator IdleAtPatrolPoint()
    {
        waiting = true;
        yield return new WaitForSeconds(Random.Range(6, 13));
        waiting = false;
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex]);
    }

    private void Flee(Vector3 boarPosition)
    {
        chasing = false;
        fleeing = true;
        Vector3 fleeDirection = transform.position - playerLastSeenAt.position;
        fleeDirection.y = 0f;
        Vector3 fleePosition = boarPosition + fleeDirection.normalized * runningDistance;
        navMeshAgent.SetDestination(fleePosition);

        if (fleeing && Vector3.Distance(transform.position, playerLastSeenAt.position) > 50f) // stop running if distance is high
        {
            //start patrolling again
            RandomPatrolPoints();
            transform.rotation = Quaternion.Euler(0,
                Quaternion.LookRotation(playerLastSeenAt.position).eulerAngles.y, 0);
            fleeing = false;
            canPatrol = true;
        }
    }

    private void Chase(Vector3 playerPosition)
    {
        fleeing = false;
        chasing = true;
        navMeshAgent.SetDestination(playerPosition);
        ManageAnimations();
    }
    private void Attack()
    {
        navMeshAgent.speed = 0f;
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            animator.SetBool(p.name, false);
        }
        for (int i = 0; i<3; i++)
        {
            if (animator.GetBool("Attack" + i.ToString()))
            {
                animator.SetBool("Attack" + i.ToString(), false);
            }
        }
        string attack = "Attack" + Random.Range(0, 3).ToString();
        animator.SetBool(attack, true);
        attacking = true;
        player.GetComponent<PlayerStats>().TakeDamage(damage);
        Vector3 directionToTarget = player.transform.position - transform.position;
        directionToTarget.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;
        StartCoroutine(WaitForAttackCooldown());
    }

    IEnumerator WaitForAttackCooldown()
    {
        yield return new WaitForSeconds(0.6f);
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            animator.SetBool(p.name, false);
        }
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
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
        else if (fleeing || chasing)
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