using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [HideInInspector] public float speed;
    public float walkSpeed;
    [SerializeField] private float sprintSpeed;
    private bool canSprint = true;

    [SerializeField] private RectTransform touchRegion;



    float horizontalMove = 0.0f;
    float verticalMove = 0.0f;

    private Rigidbody rb;
    private Animator animator = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        speed = walkSpeed;
        Observable.EveryUpdate().Subscribe(_ => Movement());

    }

    void Movement()
    {

        horizontalMove = joystick.Horizontal;
        verticalMove = joystick.Vertical;

        Vector3 movement = new Vector3(horizontalMove, 0f, verticalMove).normalized * speed;


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
            }
            else if(speed == walkSpeed)
            {
                if (canSprint)
                {
                    speed = sprintSpeed;
                }
            }
        }

    }
}