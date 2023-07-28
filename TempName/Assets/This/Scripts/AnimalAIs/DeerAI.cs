using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class DeerAI : MonoBehaviour
{
    Vector3[] patrolPoints = new Vector3[6]; // array of patrol points
    [SerializeField] private float sightDistance = 12f;
    [SerializeField] private float sensDistance = 2f;
    public LayerMask playerLayer;
    private float runningDistance = 5f;
    private Transform playerLastSeenAt;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private HealthPoints healthPoints;
    private int currentPatrolIndex = 0;
    private bool fleeing = false;
    private bool waiting = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthPoints= GetComponent<HealthPoints>();
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
        if (healthPoints.isDead)
        {
            navMeshAgent.isStopped = true;
            ManageAnimations();
            StartCoroutine(Die());
        }
        Vector3 deerPosition = transform.position;
        if (!fleeing)
        {
            Patrol();
        }
        else
        {
            Flee(deerPosition);
        }
        // Detect player
        Vector3 rayDirection = transform.forward;
        RaycastHit hit;
        Collider[] colliders = Physics.OverlapSphere(transform.position, sensDistance, playerLayer);
        if (colliders.Length > 0)
        {
            playerLastSeenAt = colliders[0].gameObject.transform;
            Flee(deerPosition);
            ManageAnimations();
        }
        else if (Physics.Raycast(deerPosition, rayDirection, out hit, sightDistance, playerLayer))
        {
            playerLastSeenAt = hit.transform;
            Flee(deerPosition);
            ManageAnimations();
        }
        else if (colliders.Length > 0)
        {
            playerLastSeenAt = colliders[0].transform;
            Flee(deerPosition);
            ManageAnimations();
        }
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

    private void Flee(Vector3 deerPosition)
    {
        fleeing = true;
        Vector3 fleeDirection = transform.position - playerLastSeenAt.position;
        fleeDirection.y = 0f;
        Vector3 fleePosition = deerPosition + fleeDirection.normalized * runningDistance;
        navMeshAgent.SetDestination(fleePosition);
    }

    IEnumerator IdleAtPatrolPoint()
    {
        waiting = true;
        yield return new WaitForSeconds(Random.Range(8, 17));
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
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        healthPoints.Edible();
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
        else if (healthPoints.isDead)
        {
            foreach(AnimatorControllerParameter p in animator.parameters)
            {
                animator.SetBool(p.name, false);
            }
            animator.SetBool("isDead", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("Idle", false);
            animator.SetBool("isWalking", true);
        }
    }
}
