using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HunterAI : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;               //  Nav mesh agent component
    public float startWaitTime = 4;                 //  Wait time of every action
    public float timeToRotate = 2;                  //  Wait time when the enemy detect near the player without seeing
    public float speedWalk = 6;                     //  Walking speed, speed in the nav mesh agent
    public float speedRun = 9;                      //  Running speed
    private Rigidbody rb;

    public float viewRadius = 5;                   //  Radius of the enemy view
    public float viewAngle = 90;                    //  Angle of the enemy view
    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;                  //  To detect the obstacules with the raycast
    public float meshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f;               //  Max distance to calcule the a minumun and a maximum raycast when hits something
    public GameObject arrowPrefab;
    public Transform arrowSpawnPosition;
 
    public List<Vector3> waypoints;                   //  All the waypoints where the enemy patrols
    int m_CurrentWaypointIndex;                     //  Current waypoint where the enemy is going to
 
    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition;                       //  Last position of the player when the player is seen by the enemy
 
    float m_WaitTime;                               //  Variable of the wait time that makes the delay
    float m_TimeToRotate;                           //  Variable of the wait time to rotate when the player is near that makes the delay
    bool m_playerInRange;                           //  If the player is in range of vision, state of chasing
    bool m_PlayerNear;                              //  If the player is near, state of hearing
    bool m_IsPatrol;                                //  If the enemy is patrol, state of patroling
    bool m_CaughtPlayer;                            //  if the enemy has caught the player
    private float timer = 0.0f;
    private float interval = 10f;
    public int numberOfWaypoints = 5; // Oluşturulacak waypoint sayısı
    public float areaSize = 10f; // Oluşturulacak waypointlerin hareket edebileceği alanın boyutu
    public float shootingRange = 5f;
    public float shootCooldown = 2f;
    private float nextShootTime = 0f;
    public float shootForce = 10f;

    void Start()
    {
        GenerateWaypoints();

        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        m_TimeToRotate = timeToRotate;
        rb = GetComponent<Rigidbody>();
 
        m_CurrentWaypointIndex = 0;                 //  Set the initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();
 
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;             //  Set the navemesh speed with the normal speed of the enemy
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex]);    //  Set the destination to the first waypoint
    }
 
    private void Update()
    {
            EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision
 
        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }
    
    private void GenerateWaypoints()
    {
        for (int i = 0; i < numberOfWaypoints; i++)
        {
            // Alanın içinde rastgele bir nokta oluştur
            Vector3 randomPosition = Random.insideUnitSphere * areaSize;
            randomPosition += transform.position;

            // Yükseklik değerini düzelt
            randomPosition.y = 0f;

            waypoints.Add(randomPosition);
        }
    }

    private void Chasing()
{
    m_PlayerNear = false;
    playerLastPosition = Vector3.zero;

    if (!m_CaughtPlayer)
    {
        Move(speedRun);
        navMeshAgent.SetDestination(m_PlayerPosition);

        // Set the "isChasing" parameter to true since the enemy is chasing the player.
        animator.SetBool("isChasing", true);

        // Check if the player is out of range or behind an obstacle
        if (Vector3.Distance(transform.position, m_PlayerPosition) > viewRadius ||
            Physics.Linecast(transform.position, m_PlayerPosition, obstacleMask))
        {
            // Player is out of range or behind an obstacle, stop chasing and go back to patrolling.
            m_IsPatrol = true;
            m_PlayerNear = false;
            m_WaitTime = startWaitTime;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex]);
            animator.SetBool("isChasing", false); // Set the "isChasing" parameter to false.
            return;
        }
        if (Vector3.Distance(transform.position, m_PlayerPosition) <= shootingRange)
        {
            m_CaughtPlayer = true;
        }


        // Check if the enemy is within shooting range and enough time has passed since the last shot.
        
    }
    else
    {
        Debug.Log(Vector3.Distance(transform.position, m_PlayerPosition));
        Stop(); // Stop moving
        animator.SetBool("isChasing", false);

        

        if (Time.time >= nextShootTime)
        {
            
            // Face the player before shooting
            Vector3 directionToPlayer = (m_PlayerPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = lookRotation;

            // Shoot an arrow
            ShootArrow();
            animator.SetTrigger("isShooting"); // Set the "isShooting" parameter to true.
            Debug.Log("Shoot");

            // Update the nextShootTime to enforce the shoot cooldown.
            nextShootTime = Time.time + shootCooldown;
            
            if (Vector3.Distance(transform.position, m_PlayerPosition) > 6)
            {
            m_IsPatrol = true;
            m_PlayerNear = false;
            m_WaitTime = startWaitTime;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex]);
            animator.SetBool("isChasing", false); // Set the "isChasing" parameter to false.
            Debug.Log("Patrol");
            return; 
            }
        }
        // The enemy caught the player, so stop chasing and set "isChasing" parameter to false.
        
    }

    // Rest of your existing code...
}
 
    private IEnumerator WaitForReset()
    {

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Reset the timer to 0f
        timer = 0f;
        
    }
    
    private void Idle()
    {
        animator.SetBool("isWalking", false);
    }

     private void ShootArrow()
    {
        // Instantiate the arrow prefab at the arrowSpawnPosition with the same rotation as the enemy.
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPosition.position, transform.rotation);

        // Get the Rigidbody component of the arrow and add force forward to simulate the shooting motion.
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();
        arrowRigidbody.AddForce(transform.forward * shootForce, ForceMode.Impulse);

    }

    private void Patroling()
    {
        
            /*
            timer += Time.deltaTime;

            // Timer, interval value passed, then run the method and start the coroutine
            if (timer >= interval)
            {
                Idle();

                // Start the coroutine to wait for 5 seconds
                StartCoroutine(WaitForReset());
            }
            */

        if (m_PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;           //  The player is no near when the enemy is patroling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex]);
            Move(speedWalk);      //  Set the enemy destination to the next waypoint
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Vector3 directionToTarget = (waypoints[m_CurrentWaypointIndex] - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100f);
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
        
                }
                else
                {
                    Stop();
                    Idle();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }
 
    private void OnAnimatorMove()
    {
 
    }
    
    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Count;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex]);
    }
 
    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
        rb.velocity = Vector3.zero;
    }
 
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
        animator.SetBool("isWalking", true);
    }
 
    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }
 
    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex]);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }
 
    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius
 
        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;             //  The player has been seeing by the enemy and then the enemy starts to chasing the player
                    m_IsPatrol = false;                 //  Change the state to chasing the player
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                m_playerInRange = false;                //  Change the sate of chasing
            }
            if (m_playerInRange)
            {
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
    }
}