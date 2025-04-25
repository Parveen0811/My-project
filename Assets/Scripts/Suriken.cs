using UnityEngine;
using System.Collections;

public class Suriken : MonoBehaviour
{
    public GameObject BloodPrefab; // Prefab for blood effect
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Do nothing if this rocket is a ghost
        if (Trajectory.IsGhost) return;

        Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();

        // Add a FixedJoint2D to the Suriken and connect it with the colliding object
        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = otherRb;
        Debug.Log("Suriken joined with " + collision.gameObject.name);

        if(collision.gameObject.CompareTag("Enemy")) // Check if hit an enemy
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

        // If collision occurs before splitting, simply destroy the suriken
        Destroy(gameObject, 5f);
    }

}
