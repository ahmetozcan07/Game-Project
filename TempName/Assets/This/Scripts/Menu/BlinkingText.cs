using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProText;

    [SerializeField] private float onDuration;
    [SerializeField] private float offDuration;

    private bool isTextVisible = true;
    private float timer;

    private void Start()
    {
        timer = onDuration;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isTextVisible = !isTextVisible;

            textMeshProText.enabled = isTextVisible;

            timer = isTextVisible ? onDuration : offDuration;
        }
    }
}
