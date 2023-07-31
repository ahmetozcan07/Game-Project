using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SurvivalTime : MonoBehaviour

{
    public TextMeshProUGUI timeText;
    private float gameTime;
    private bool isGameRunning = true;
    private string time;

    private void Start()
    {
        gameTime = 0f;
    }

    private void Update()
    {
        if (isGameRunning)
        {
            gameTime += Time.deltaTime;
            UpdateTimeText();
        }
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        time = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text = time;
    }


    public void GoMenu()
    {
        GameObject bridge = GameObject.FindGameObjectWithTag("Bridge");
        Bridge bridgeScript = bridge.GetComponent<Bridge>();
        bridgeScript.SetTime(time);
        bridgeScript.first = false;

        SceneManager.LoadScene("Menu");

    }




}

