using UnityEngine;

public class Knife : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject BloodPrefab; // Prefab for blood effect


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on the Knife prefab!");
        }
    }

    private void Update()
    {
        // Only update rotation if the knife hasn't collided
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Trajectory.IsGhost) return;// Do nothing if this knife is a ghost

        Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();

        // Add a FixedJoint2D to the Suriken and connect it with the colliding object
        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = otherRb;
        Debug.Log("Suriken joined with " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Enemy")) // Check if hit an enemy
        {
            // Instantiate the blood effect and ensure it renders above the suriken
            GameObject effect = Instantiate(BloodPrefab, transform.position, Quaternion.identity);
            Renderer effectRenderer = effect.GetComponent<Renderer>();
            if (effectRenderer != null)
            {
                effectRenderer.sortingOrder = 2; // Ensure the particle system renders above the suriken
            }

            Destroy(gameObject, 0.5f);
        }

        Destroy(gameObject, 0.5f);
    }
}
