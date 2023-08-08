using Lean.Touch;
using UniRx;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform touchRegion;

    public float walkSpeed;
    public float sprintSpeed;
    public bool canWalk = true;
    private bool canSprint = true;

    [HideInInspector] public float speed;
    [HideInInspector] public bool isSprinting = true;
    [HideInInspector] public Vector3 movement = new Vector3 ();

    private float horizontalMove = 0;
    private float verticalMove = 0;

    private Rigidbody rb;
    private Animator animator;
    private PlayerStats playerStats;
    private float raycastDistance = 3f;
    private float rotationSpeed = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        speed = walkSpeed;
        Observable.EveryUpdate().Subscribe(_ => Movement()).AddTo(this);
        Observable.EveryUpdate().Subscribe(_ => Rotation()).AddTo(this);
    }

    void Movement()
    {


        horizontalMove = joystick.Horizontal;
        verticalMove = joystick.Vertical;

        if (!playerStats.isDead && canWalk)
        {
            movement = new Vector3(horizontalMove, 0f, verticalMove).normalized * speed;

            if (movement != Vector3.zero)
            {
                transform.forward = movement;
                animator.SetBool("Idle", false);
                animator.SetBool("Run Forward", true);

            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Run Forward", false);
            }

            SprintCheck();

            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        }









/*
        horizontalMove = joystick.Horizontal;
        verticalMove = joystick.Vertical;

        movement = new Vector3(horizontalMove, 0f, verticalMove).normalized * speed;

        if (movement != Vector3.zero && !playerStats.isDead && canWalk)
        {
            transform.forward = movement;
            animator.SetBool("Idle", false);
            animator.SetBool("Run Forward", true);
        }
        else if(!playerStats.isDead)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Run Forward", false);
        }

        SprintCheck();

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);*/
    }

    void Rotation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            Vector3 moveDirection = hit.normal;
            float groundAngle = Vector3.Angle(moveDirection, rb.transform.forward);
            Quaternion targetRotation = transform.rotation * Quaternion.Euler((90 - groundAngle), 0f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerTap += OnFingerDown;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= OnFingerDown;
    }

    private void OnFingerDown(LeanFinger finger)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(touchRegion, finger.ScreenPosition))
        {
            if(speed == sprintSpeed)
            {
                speed = walkSpeed;
                isSprinting = false;
            }
            else if(speed == walkSpeed)
            {
                if (canSprint)
                {
                    speed = sprintSpeed;
                    isSprinting = true;
                }
            }
        }

    }

    public void SprintCheck()
    {
        if (playerStats.health < 70 || playerStats.hunger < 10)
        {
            canSprint = false;
            speed = walkSpeed;
            isSprinting = false;
        }
        else
        {
            canSprint = true;
        }
    }
}