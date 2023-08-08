using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image hungerBar;
    [SerializeField] private Image staminaBar;

    [HideInInspector] public float health = 100;
    [HideInInspector] public float hunger = 100;
    [HideInInspector] public float stamina = 100;

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
        FixStats();


        if (health > 0)
        {

            if (playerMovement.movement == Vector3.zero)
            {
                stamina += 8f * Time.deltaTime;
            }
            else if (playerMovement.isSprinting)
            {
                stamina -= 8f * Time.deltaTime;
            }
            else
            {
                stamina -= 4f * Time.deltaTime;
            }
        }
        if (health < 0){ stamina = 0f; }


        if (hunger > 0)
        {
            if (stamina > 75)
            {
                hunger -= 1f * Time.deltaTime;
            }
            else if (stamina > 50)
            {
                hunger -= 2f * Time.deltaTime;
            }
            else if (stamina > 25)
            {
                hunger -= 3f * Time.deltaTime;
            }
            else
            {
                hunger -= 4f * Time.deltaTime;
            }

            if (hunger > 50)
            {
                health += 2 * Time.deltaTime;
                FixStats();
            }

        }

   
        if (hunger == 0)
        {
            health -= healthDecrease * Time.deltaTime;
            healthDecrease += 0.5f * Time.deltaTime;
        }
        if (health <= 0)
        {
            Die();
        }

        healthBar.fillAmount = health / 100;
        hungerBar.fillAmount = hunger / 100;
        staminaBar.fillAmount = stamina / 100;
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        FixStats();
    }
    public void GetHealed(float healing)
    {
        health += healing;
        FixStats();
    }
    public void GetFed(float food)
    {
        hunger += food;
        FixStats();
    }

    public void GetStamina(float breath)
    {
        stamina += breath;
        FixStats();
    }





    void FixStats()
    {
        if (stamina > 100)
        {
            stamina = 100;
        }

        if (hunger > 100)
        {
            hunger = 100;
        }

        if (health > 100)
        {
            health = 100;
        }

        if (stamina < 0)
        {
            stamina = 0;
        }

        if (hunger < 0)
        {
            hunger = 0;
        }

        if (health < 0)
        {
            health = 0;
        }

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
