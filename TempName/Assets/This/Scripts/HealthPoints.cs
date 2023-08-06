using System.Collections;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [HideInInspector] public float health;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isEdible = false;
    private Animator animator;


    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
        }
        Debug.Log("hasar yedi: " + damage);
    }

    public void Edible()
    {
        isEdible = true;
    }

    IEnumerator Died()
    {
        yield return new WaitForSeconds(1);
        Edible();
    }

    public void Die()
    {
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            animator.SetBool(p.name, false);
        }
        animator.SetBool("isDead", true);
        StartCoroutine(Died());
    }
}
