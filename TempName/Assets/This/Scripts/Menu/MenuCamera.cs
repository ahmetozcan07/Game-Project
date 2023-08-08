using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{

    private float speed = 100f;
    public bool doing = false;
    public bool done = false;
    public bool first = true;

    void Update()
    {
        if (doing && !done)
        { CameraMove(); }
    }
    
    public void CameraMove()
    {
        if(first)
        {
            Moved();
            first = false;
        }
        if (transform.eulerAngles.x > 270)
        {
            Moved();
        }
        else
        {
            doing = false;
            transform.eulerAngles = new Vector3(270, transform.eulerAngles.y, transform.eulerAngles.z);
            done = true;
        }
    }

    public void Moved()
    {
        Vector3 newRotation = transform.eulerAngles;
        newRotation.x -= speed * Time.deltaTime;
        transform.eulerAngles = newRotation;
    }

 
}
