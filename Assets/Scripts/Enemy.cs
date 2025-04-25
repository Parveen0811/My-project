using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball")) // Check if hit by a ball
        {
            TakeDamage(50); // Deduct 20 HP (adjust as needed)
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Ensure health doesn't go below 0

        if (currentHealth <= 0)
        {
            Die(); // Handle enemy death
        }
    }

    private void Die()
    {
        Destroy(gameObject); // Destroy the enemy
    }
}

