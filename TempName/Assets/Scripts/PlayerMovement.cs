using Lean.Touch;
using UniRx;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform touchRegion;
    public float walkSpeed;
    public float sprintSpeed;
    private bool canSprint = true;
    [HideInInspector] public float speed;
    [HideInInspector] public bool isSprinting = true;
    [HideInInspector] public Vector3 movement = new Vector3 ();
    private float horizontalMove = 0;
    private float verticalMove = 0;
    private Rigidbody rb;
    private Animator animator;
    private PlayerStats playerStats;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        speed = walkSpeed;
        Observable.EveryUpdate().Subscribe(_ => Movement());
    }

    void Movement()
    {

        horizontalMove = joystick.Horizontal;
        verticalMove = joystick.Vertical;

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

        if(playerStats.health < 70 || playerStats.hunger < 10)
        {
            canSprint = false;
            speed = walkSpeed;
            isSprinting = false;
        }
        else
        {
            canSprint = true;
        }

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
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
}