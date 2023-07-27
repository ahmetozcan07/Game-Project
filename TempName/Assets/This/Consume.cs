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

    private ColliderObject CheckClosestObject()
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
   
    private void PerformActionForRegion() //tu�a bas�ld���nda yap
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
    }
}
