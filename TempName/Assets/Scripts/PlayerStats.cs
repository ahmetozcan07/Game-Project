using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image hungerBar;
    [HideInInspector] public float health = 100;
    [HideInInspector] public float hunger = 100;
    private float healthDecrease;
    private PlayerMovement playerMovement;

    private void Start()
    {
        health = 80;
        hunger = 100;
        healthDecrease = 2;
        playerMovement = GetComponent<PlayerMovement>();

        Observable.EveryUpdate()
            .Subscribe(_ => UpdateStats());
    }

    private void UpdateStats()
    {
        if (hunger > 0)
        {
            if(playerMovement.movement == Vector3.zero)
            {
                hunger -= 0.5f * Time.deltaTime;
            }
            else if(playerMovement.isSprinting)
            {
                hunger -= 4 * Time.deltaTime;
            }
            else
            {
                hunger -= 2 * Time.deltaTime;
            }
            if (hunger > 50)
            {
                health += 2 * Time.deltaTime;
            }
        }
        else
        {
            hunger = 0;
        }
        if (hunger == 0)
        {
            health -= healthDecrease * Time.deltaTime;
            healthDecrease += 1 * Time.deltaTime;
        }
        if (health <= 0)
        {
            Die();
        }
        healthBar.fillAmount = health / 100;
        hungerBar.fillAmount = hunger / 100;
    }

    private void Die()
    {
        // play dying animation
        Destroy(gameObject);
    }

}
