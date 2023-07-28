using UnityEngine;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private RectTransform touchRegion;
    [SerializeField] private float damage;
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private Consume consume;
    //private List<GameObject> collidingObjects = new List<GameObject>();
    private bool attacking = false;
    //private bool didAttack = false;
    private float attackCooldown = 0.6f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        consume = GetComponent<Consume>();
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
            if (!attacking)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        ColliderObject closestObject = consume.CheckClosestObject();
        if (closestObject != null)
        {
            if (closestObject.Tag == "MEAT")
            {
                GameObject go = closestObject.Object.gameObject.transform.parent.gameObject;

                Vector3 targetPosition = closestObject.Object.transform.position;
                Vector3 directionToTarget = targetPosition - transform.position;
                directionToTarget.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = targetRotation;
                StartCoroutine(AttackAnim(go));
            }
            else
            {
                StartCoroutine(AttackAnim());
            }
        }
        else
        {
            StartCoroutine(AttackAnim());
        }
    }

    private void DealDamage(GameObject go)
    {
        go.GetComponent<HealthPoints>().TakeDamage(damage);
    }

    IEnumerator AttackAnim()
    {
        int attackStyle = Random.Range(1, 5);
        if (attackStyle == 4)
        {
            attackStyle = 5;
        }
        string attack = "Attack" + attackStyle.ToString();
        animator.SetTrigger(attack);
        yield return new WaitForSeconds(0.2f);
        attacking = true;
        playerMovement.speed = 0;
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
        if (playerMovement.isSprinting)
        {
            playerMovement.speed = playerMovement.sprintSpeed;
        }
        else
        {
            playerMovement.speed = playerMovement.walkSpeed;
        }
    }
    IEnumerator AttackAnim(GameObject go)
    {
        int attackStyle = Random.Range(1, 5);
        if (attackStyle == 4)
        {
            attackStyle = 5;
        }
        string attack = "Attack" + attackStyle.ToString();
        animator.SetTrigger(attack);
        yield return new WaitForSeconds(0.2f);
        attacking = true;
        playerMovement.speed = 0;
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
        DealDamage(go);
        if (playerMovement.isSprinting)
        {
            playerMovement.speed = playerMovement.sprintSpeed;
        }
        else
        {
            playerMovement.speed = playerMovement.walkSpeed;
        }
    }


    //private void OnTriggerStay(Collider other)
    //{
    //    if (!collidingObjects.Contains(other.gameObject))
    //    {
    //        collidingObjects.Add(other.gameObject);
    //    }
    //    if (attacking && !didAttack)
    //    {
    //        foreach (var obj in collidingObjects)
    //        {
    //            if (obj != null)
    //            {
    //                Debug.Log("damage done");
    //                if (obj.GetComponent<HealthPoints>().isDead)
    //                {
    //                    if (obj.layer == 8) // rabbit
    //                    {
    //                        playerStats.hunger += 10;
    //                    }
    //                    else if (obj.layer == 9) // deer
    //                    {
    //                        playerStats.hunger += 40;
    //                    }
    //                    else if (obj.layer == 10) // boar
    //                    {
    //                        playerStats.hunger += 40;
    //                    }
    //                }
    //                obj.GetComponent<HealthPoints>().TakeDamage(damage);
    //                didAttack = true;
    //            }
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    collidingObjects.Remove(other.gameObject);
    //}
}