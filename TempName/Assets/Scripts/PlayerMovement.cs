using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 1.0f;

    float horizontalMove = 0.0f;
    float verticalMove = 0.0f;

    private Rigidbody rb;
    private Animator animator = null;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();


        Observable.EveryUpdate().Subscribe(_ => Movement()).AddTo(this);
    }

    void Movement()
    {
        horizontalMove = joystick.Horizontal * speed;

        verticalMove = joystick.Vertical * speed;

        Vector3 movement = new Vector3(horizontalMove, 0f, verticalMove);

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Run Forward");
        }
        else
        {
            animator.ResetTrigger("Run Forward");
            animator.SetTrigger("Idle");
        }

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }
}