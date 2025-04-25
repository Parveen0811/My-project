using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 5f); // Destroy the ball on collision   
    }
}
