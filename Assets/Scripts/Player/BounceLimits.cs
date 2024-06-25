using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceLimits : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float bounceForce = 10f; // Força do bounce
    [SerializeField] private float xLimit = 9f; // Limite da borda no eixo X
    [SerializeField] private float yLimit = 5f; // Limite da borda no eixo Y

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckBounds();
    }

    void CheckBounds()
    {
        Vector2 velocity = rb.velocity;
        
        if (transform.position.x < -xLimit || transform.position.x > xLimit)
        {
            velocity.x = -velocity.x;
            Bounce(Vector2.right);
        }
        if (transform.position.y < -yLimit || transform.position.y > yLimit)
        {
            velocity.y = -velocity.y;
            Bounce(Vector2.up);
        }

        rb.velocity = velocity;
    }

    void Bounce(Vector2 direction)
    {
        rb.AddForce(-direction * bounceForce, ForceMode2D.Impulse);
    }
}
