using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YFix : MonoBehaviour
{
    public GameObject FixedJoystick;

    public GameObject StaminaBar;
    public GameObject HungerBar;
    public GameObject HealthBar;

    public GameObject AttackButtonRenderer;
    public GameObject SprintButtonRenderer;
    public GameObject InteractionButtonRenderer;

    public GameObject Image;
    public GameObject SurvivalTimeText;

    private void Start()
    {
        Ccc(FixedJoystick, 320f);

        Ccc(StaminaBar, -190f);
        Ccc(HungerBar, -135f);
        Ccc(HealthBar, -75f);

        Ccc(AttackButtonRenderer, 300f);
        Ccc(SprintButtonRenderer, 350f);
        Ccc(InteractionButtonRenderer, 100f);

        Ccc(Image, -160f);
        Ccc(SurvivalTimeText, -75f);

    }

    private void Ccc(GameObject targetObject, float Y)
    {
        RectTransform rectTransform = targetObject.GetComponent<RectTransform>();

        Vector2 newPosition = rectTransform.anchoredPosition;
        newPosition.y = Y;
        rectTransform.anchoredPosition = newPosition;

    }

    
}

