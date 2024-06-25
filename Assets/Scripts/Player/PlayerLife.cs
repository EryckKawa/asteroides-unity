using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int life;
    [SerializeField] private float repulsionForce = 10f; // A força de repulsão quando o jogador colide com um meteoro

    public int Life
    {
        get { return life; }
        private set { life = value; }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        life--;
        Debug.Log(life);

        // Verifica se o outro objeto tem um Rigidbody2D
        Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb != null)
        {
            // Calcula a direção da força de repulsão
            Vector2 repulsionDirection = (other.transform.position - transform.position).normalized;

            // Aplica a força de repulsão ao jogador e ao meteoro
            GetComponent<Rigidbody2D>().AddForce(-repulsionDirection * repulsionForce, ForceMode2D.Impulse);
            otherRb.AddForce(repulsionDirection * repulsionForce, ForceMode2D.Impulse);
        }
    }
}
