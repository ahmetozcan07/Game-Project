using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class RabbitAI : MonoBehaviour
{
    Vector3[] patrolPoints = new Vector3[5]; // array of patrol points
    [SerializeField] private float sightDistance = 12f;
    public LayerMask playerLayer;
    private float fleeDistance = 5f;
    private Transform playerLastSeenAt;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int currentPatrolIndex = 0;
    private bool fleeing = false;
    private bool waiting = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] = new Vector3(0, 0, 0);
        }
        RandomPatrolPoints();

        // Walk to one of the patrol points
        if (patrolPoints.Length > 0)
        {
            navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex]);
            Debug.Log(patrolPoints[currentPatrolIndex] + " " + patrolPoints[1]);
        }


        Observable.EveryUpdate()
            .Subscribe(_ => ManageAnimations());
        Observable.EveryUpdate()
            .Subscribe(_ => PlayerSeen());
    }

    private void PlayerSeen()
    {
        // Position of the rabbit
        Vector3 rabbitPosition = transform.position;

        // If not running, patrol
        if (!fleeing)
        {
            Patrol();
        }
        else
        {
            Flee(rabbitPosition);
        }

        // Detect player
        Vector3 rayDirection = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(rabbitPosition, rayDirection, out hit, sightDistance, playerLayer))
        {
            playerLastSeenAt = hit.transform;
            // Player seen, run
            fleeing = true;
            Flee(rabbitPosition);
            ManageAnimations();
        }
        else if (fleeing && Vector3.Distance(transform.position, playerLastSeenAt.position) > 50f) // stop running if distance is high
        {
            //start patrolling again
            RandomPatrolPoints();
            fleeing = false;
            transform.rotation = Quaternion.Euler(0,
                Quaternion.LookRotation(playerLastSeenAt.position).eulerAngles.y, 0);
        }
    }

    private void Patrol()
    {
        // Walk to next patrol point when arrived at one of patrol points
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f && !waiting && !fleeing)
        {
            navMeshAgent.updateRotation = false;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            StartCoroutine(IdleAtPatrolPoint());
        }
    }

    private void Flee(Vector3 rabbitPosition)
    {
        Vector3 fleeDirection = transform.position - playerLastSeenAt.position;
        fleeDirection.y = 0f;
        Vector3 fleePosition = rabbitPosition + fleeDirection.normalized * fleeDistance;
        navMeshAgent.SetDestination(fleePosition);
    }

    IEnumerator IdleAtPatrolPoint()
    {
        waiting = true;
        yield return new WaitForSeconds(Random.Range(6,13));
        waiting = false;
        navMeshAgent.updateRotation = true;
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex]);
    }

    private void RandomPatrolPoints()
    {
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] = new Vector3(transform.position.x + Random.Range(6, 32), 0,
                transform.position.z + Random.Range(6, 32));
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
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("Idle", false);
        }
    }
}