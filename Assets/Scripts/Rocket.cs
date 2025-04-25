using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionPrefab; // Reference to the explosion prefab
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on the Rocket prefab!");
        }
    }

    private void Update()
    {
        // Update the rocket's rotation to align with its velocity
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f) // Check if the rocket is moving
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Do nothing if this rocket is a ghost
        if (Trajectory.IsGhost) return;

        // Instantiate the explosion effect
        if (explosionPrefab != null)
        {
            GameObject effect = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f); // Cleanup the explosion effect after 2 seconds
        }
        else
        {
            Debug.LogError("Explosion prefab is not assigned to the Rocket prefab!");
        }

        // Apply explosion force to nearby objects
        ApplyExplosionForce(100f, transform.position, 1f);

        Destroy(gameObject, 0.1f); // Destroy the rocket after 0.1 seconds
    }

    private void ApplyExplosionForce(float force, Vector2 explosionPosition, float radius)
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, radius);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D nearbyRb = collider.GetComponent<Rigidbody2D>();
            if (nearbyRb != null)
            {
                Vector2 direction = (nearbyRb.position - explosionPosition).normalized;
                float distance = Vector2.Distance(nearbyRb.position, explosionPosition);
                float explosionEffect = 1 - (distance / radius); // Scale force based on distance
                nearbyRb.AddForce(direction * force * explosionEffect, ForceMode2D.Impulse);
            }
        }
    }
}
