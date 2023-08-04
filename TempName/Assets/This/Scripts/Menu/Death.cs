using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Death : MonoBehaviour
{
    public Transform endPanel;
    public Transform mainMenu;
    public Transform loadScreen;

    void Start()
    {

        GameObject bridge = GameObject.FindGameObjectWithTag("Bridge");
        Bridge bridgeScript = bridge.GetComponent<Bridge>();

        if (bridgeScript.first)
        {
            endPanel.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
            loadScreen.gameObject.SetActive(false);
        }
        else
        {
            DeathTasks();
        }
       
      
    }

    public void DeathTasks()
    {
        endPanel.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);

        GameObject bridge = GameObject.FindGameObjectWithTag("Bridge");
        Bridge bridgeScript = bridge.GetComponent<Bridge>();
    }
    

}
