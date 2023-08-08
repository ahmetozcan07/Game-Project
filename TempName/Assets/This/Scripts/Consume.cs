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

    public ColliderObject theObject;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        ColliderObject newColliderObject = new ColliderObject(other.gameObject.tag, other.gameObject);
        interactableObjects.Add(newColliderObject);
    }
    private void OnTriggerExit(Collider other)
    {
        ColliderObject removedObject = interactableObjects.Find(obj => obj.Object == other.gameObject);
        if (removedObject != null)
        {
            interactableObjects.Remove(removedObject);
        }
    }

    public void CheckClosestObject()
    {
        ColliderObject closestObject = null;

        if (interactableObjects.Count > 0)
        {
            float closestDistance = Mathf.Infinity;
            Vector3 characterPosition = transform.position;

            foreach (ColliderObject colliderObj in interactableObjects)
            {

                if (colliderObj.Object != null)
                {
                    float distance = Vector3.Distance(colliderObj.Object.transform.position, characterPosition);
                    if (distance < closestDistance)
                    {
                        closestObject = colliderObj;
                        closestDistance = distance;
                    }
                }


            }

        }

        theObject = closestObject;
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
            CheckClosestObject();
            PerformActionForObject();
        }
    }

    private void PerformActionForObject()
    {

        if (theObject != null)
        {
            switch (theObject.Tag)
            {
                case "M1":
                    EatMushroom1();
                    break;
                case "M2":
                    EatMushroom2();
                    break;
                case "M3":
                    EatMushroom3();
                    break;
                case "M4":
                    EatMushroom4();
                    break;
                case "G1":
                    EatGrass1();
                    break;
                case "G2":
                    EatGrass2();
                    break;
                case "MEAT":
                    //EatMeat();
                    break;
                default:
                    break;
            }


        }


    }


    void EatMeat()
    {
        GameObject go = theObject.Object.gameObject.transform.parent.gameObject;
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

      
        Eat();

        //meat tipinde bir objeye d�n��ecek ve bir miktar azalacak bitince ytok olur,
        //bu meat tipi bu hayvanlar�n bir alt objesi script falan ayn� sadece meat miktar� farkl�
        //ve sadece hayvan �l� moda ge�ince a��l�r
    }
   
    void EatMushroom1()
    {
        Eat();

    }
    void EatMushroom2()
    {
        Eat();
    }
    void EatMushroom3()
    {
        Eat();
    }
    void EatMushroom4()
    {
        Eat();
    }
    void EatGrass1()
    {
        Eat();
        playerStats.GetStamina(50f);
    }
    void EatGrass2()
    {
        Eat();
        playerStats.GetHealed(25f);
    }
    void Eat()
    {
        TurnToMeal();

        StartCoroutine(EatAnimation());
        
        DestroyTheObject();

        CheckClosestObject();
    }

    IEnumerator EatAnimation()
    {
        animator.SetBool("Eat", true);
        playerMovement.canWalk = false;

        yield return new WaitForSeconds(1.5f);

        animator.SetBool("Eat", false);
        playerMovement.canWalk = true;
        playerMovement.SprintCheck();

    }

    void TurnToMeal()
    {
        Vector3 targetPosition = theObject.Object.transform.position;
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;
    }
    
    void DestroyTheObject()
    {
       
        ColliderObject removedObject = interactableObjects.Find(obj => obj.Object == theObject.Object);

        if (removedObject != null)
        {
            interactableObjects.Remove(removedObject);
        }

        GameObject.Destroy(theObject.Object.transform.parent.gameObject);

        //parent� as�l prefab oluyor, ama geri kalan er i�lem ve interactableObjectste tutlan object as�l prefab de�il
        theObject = null;
    }


}
