using UnityEngine;

public class Plank : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball")) // Check if hit by a ball
        {
            Destroy(gameObject); // Destroy the plank
        }
    }
}
