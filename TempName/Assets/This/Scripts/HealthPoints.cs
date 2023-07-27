using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [HideInInspector] public float health;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);    
    }
}
