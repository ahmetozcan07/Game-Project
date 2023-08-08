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
    private Animator animator;

    [HideInInspector] public bool isDead = false;

    private void Start()
    {
        healthDecrease = 2;
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        Observable.EveryUpdate()
            .Subscribe(_ => UpdateStats()).AddTo(this);
    }

    private void UpdateStats()
    {
        if (hunger > 0)
        {
            if(hunger >= 100)
            {
                hunger = 100;
            }
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
                if (health >= 100)
                {
                    health = 100;
                }
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
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    public void GetHealed(float healing)
    {
        health += healing;
    }
    public void GetFed(float food)
    {
        hunger += food;
    }

    private void Die()
    {
        isDead = true;
        for(int i = 0; i < animator.parameterCount; i++)
        {
            animator.SetBool(i, false);
        }
        StartCoroutine(DieAnim());
    }

    IEnumerator DieAnim()
    {
        playerMovement.speed = 0;
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            animator.SetBool(p.name, false);
        }
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(4);

        SurvivalTime survivalTimeScript = GetComponent<SurvivalTime>();
        survivalTimeScript.GoMenu();
    }
}
