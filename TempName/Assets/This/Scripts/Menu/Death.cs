using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Death : MonoBehaviour
{
    
    void Start()
    {

        GameObject bridge = GameObject.FindGameObjectWithTag("Bridge");
        Bridge bridgeScript = bridge.GetComponent<Bridge>();

     
        if (bridgeScript.first)
        {

            GameObject panel = GameObject.FindGameObjectWithTag("Canvas");
            panel.transform.GetChild(2).gameObject.SetActive(false);
            panel.transform.GetChild(1).gameObject.SetActive(true);

        }
        else
        {
            DeathTasks();
        }
       
      
    }

    public void DeathTasks()
    {

        GameObject panel = GameObject.FindGameObjectWithTag("Canvas");
        panel.transform.GetChild(2).gameObject.SetActive(true);
        panel.transform.GetChild(1).gameObject.SetActive(false);

       
        GameObject bridge = GameObject.FindGameObjectWithTag("Bridge");
        Bridge bridgeScript = bridge.GetComponent<Bridge>();
      
    }
    

}
