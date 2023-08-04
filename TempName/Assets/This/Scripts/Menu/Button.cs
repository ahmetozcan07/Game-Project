using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public Transform endPanel;
    public Transform mainMenu;
    public Transform loadScreen;

    public void PlayGame()
    {
        StartCoroutine(PlayGameCoroutine());
    }

    public IEnumerator PlayGameCoroutine()
    {
        mainMenu.gameObject.SetActive(false);

        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        MenuCamera cameraScript = mainCamera.GetComponent<MenuCamera>();
        cameraScript.doing = true;

        yield return new WaitUntil(() => cameraScript.done);

        loadScreen.gameObject.SetActive(true);

        SceneManager.LoadScene("GameScene");
    }



    public void QuitGame()
    {
        Application.Quit();
    }


    public void HideEndPanel()
    {

        endPanel.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);

    }



}

