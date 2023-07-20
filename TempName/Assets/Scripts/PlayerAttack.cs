using UnityEngine;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]private RectTransform touchRegion;
    private Animator animator;
    private PlayerMovement playerMovement;


    private List<GameObject> collidingObjects = new List<GameObject>();
    private bool canAttack = true;
    private bool didAttack = false;
    private float attackThreshold = 0.7f;

    //private bool isThereAnEnemy = false;
    //private int collidingCount = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
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
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        int attackStyle = Random.Range(1, 5);
        if (attackStyle == 4)
        {
            attackStyle = 5;
        }
        string attack = "Attack" + attackStyle.ToString();

        animator.SetTrigger(attack);

        canAttack = false;
        playerMovement.speed = 0;
        yield return new WaitForSeconds(attackThreshold);
        canAttack = true;
        didAttack = false;
        playerMovement.speed = playerMovement.walkSpeed;
    }


    private void OnTriggerStay(Collider other)
    {

        if (!collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Add(other.gameObject);
        }
        
        if (!canAttack && !didAttack)
        {
            foreach (var obj in collidingObjects)
            {
                if(obj != null)
                {
                    Debug.Log("damage done");
                    obj.GetComponent<HealthPoints>().TakeDamage(20);
                    didAttack = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collidingObjects.Remove(other.gameObject);
    }
}