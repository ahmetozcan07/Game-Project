using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderObject
{
    public string Tag { get; private set; }
    public GameObject Object { get; private set; }

    public ColliderObject(string tag, GameObject obj)
    {
        Tag = tag;
        Object = obj;
    }
}

public class Consume : MonoBehaviour
{
    public List<ColliderObject> interactableObjects = new List<ColliderObject>();
    [SerializeField] private RectTransform touchRegion;
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        ColliderObject newColliderObject = new ColliderObject(otherObject.tag, otherObject);
        interactableObjects.Add(newColliderObject);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherObject = other.gameObject;
        ColliderObject removedObject = interactableObjects.Find(obj => obj.Object == otherObject);
        if (removedObject != null)
        {
            interactableObjects.Remove(removedObject);
        }
    }

    public ColliderObject CheckClosestObject()
    {
        ColliderObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        Vector3 characterPosition = transform.position;

        foreach (ColliderObject colliderObj in interactableObjects)
        {
            float distance = Vector3.Distance(colliderObj.Object.transform.position, characterPosition);
            if (distance < closestDistance)
            {
                closestObject = colliderObj;
                closestDistance = distance;
            }
        }

        return closestObject;       
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
            PerformActionForRegion();
        }
    }

    private void PerformActionForRegion()
    {
        ColliderObject closestObject = CheckClosestObject();

        if(closestObject != null)
        {
            switch (closestObject.Tag)
            {
                case "M1":
                    EatMushroom1(closestObject);
                    break;
                case "M2":
                    EatMushroom2(closestObject);
                    break;
                case "M3":
                    EatMushroom3(closestObject);
                    break;
                case "M4":
                    EatMushroom4(closestObject);
                    break;
                case "G1":
                    EatGrass1(closestObject);
                    break;
                case "G2":
                    EatGrass2(closestObject);
                    break;
                case "MEAT":
                    EatMeat(closestObject);
                    break;
                default:
                    break;
            }
        }
    }



    void EatMeat(ColliderObject closestObject)
    {
        GameObject go = closestObject.Object.gameObject.transform.parent.gameObject;
        if (go.GetComponent<HealthPoints>().isEdible)
        {
            //hayvan t�r�ne g�re farkl� etki
            if (go.layer == 8) // rabbit
            {

            }
            else if (go.layer == 9) // deer
            {

            }
            else if (go.layer == 10) // boar
            {

            }
        }
        Eat(closestObject);
        interactableObjects.Remove(closestObject);
        //meat tipinde bir objeye d�n��ecek ve bir miktar azalacak bitince ytok olur, bu meat tipi bu hayvanlar�n bir alt objesi script falan ayn� sadece meat miktar� farkl�
        //ve sadece hayvan �l� moda ge�ince a��l�r
    }

    void EatMushroom1(ColliderObject closestObject)
    {
        Eat(closestObject);
        //etkileri
        //de�erler i�in  veya �zelliklerinde
        interactableObjects.Remove(closestObject);
    }
    void EatMushroom2(ColliderObject closestObject)
    {
        Eat(closestObject);
        interactableObjects.Remove(closestObject);
    }
    void EatMushroom3(ColliderObject closestObject)
    {
        Eat(closestObject);
        interactableObjects.Remove(closestObject);
    }
    void EatMushroom4(ColliderObject closestObject)
    {
        Eat(closestObject);
        interactableObjects.Remove(closestObject);
    }
    void EatGrass1(ColliderObject closestObject)
    {
        Eat(closestObject);
        interactableObjects.Remove(closestObject);
    }
    void EatGrass2(ColliderObject closestObject)
    {
        Eat(closestObject);
        interactableObjects.Remove(closestObject);
    }
    void Eat(ColliderObject closestObject)
    {
     //animasyon da buraya gelmeli

      
        Vector3 targetPosition = closestObject.Object.transform.position;
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0f; // E�imden kaynakl� y�kseklik fark�n� dikkate almayaca��z.

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;

        //animasyon
        StartCoroutine(Eat(closestObject.Object.gameObject.transform.parent.gameObject));
    }

    IEnumerator Eat(GameObject obj)
    {
        animator.SetBool("Eat", true);
        playerMovement.speed = 0f;
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("Eat", false);

        if (playerMovement.isSprinting)
        {
            playerMovement.speed = playerMovement.sprintSpeed;
        }
        else
        {
            playerMovement.speed = playerMovement.walkSpeed;
        }
    }
}
