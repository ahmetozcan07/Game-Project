using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public static Bridge instance;
    public string time;
    public bool first = true;

    void Start()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
          
        }

        else
        {
            Destroy(gameObject);
        }

    }

    public void SetTime(string time)
    {
        this.time = time;
    }

    public string GetTime()
    {
        return time;
    }



}
