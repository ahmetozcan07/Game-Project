using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [HideInInspector] public float health;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isEdible = false;


    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
        }
    }

    public void Edible()
    {
        isEdible = true;
    }
}
