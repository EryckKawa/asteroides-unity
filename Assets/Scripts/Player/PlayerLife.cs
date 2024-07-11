using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int life;
    [SerializeField] private float repulsionForce = 10f;
    [SerializeField] private AudioSource collisionSound;
    private SpriteRenderer playerVisual;
    private PlayerHealth playerHealth;
    private GameManager gameManager;

    private void Start()
    {
        playerVisual = transform.Find("PlayerVisual").GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
        gameManager = FindObjectOfType<GameManager>();
        life = 3;
    }

    public int Life
    {
        get { return life; }
        private set { life = value; }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            collisionSound.Play();
        }

        life--;
        Debug.Log(life);

        Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb != null)
        {
            Vector2 repulsionDirection = (other.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(-repulsionDirection * repulsionForce, ForceMode2D.Impulse);
            otherRb.AddForce(repulsionDirection * repulsionForce, ForceMode2D.Impulse);
        }

        AnimateDamage();
        playerHealth.UpdateHealthUI(life);

        if (life <= 0)
        {
            gameManager.TriggerGameOver();
        }
    }

    void AnimateDamage()
    {
        playerVisual.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            playerVisual.DOColor(Color.white, 0.1f);
        });
        playerVisual.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.3f, 10, 1);
    }

    public void ResetLife(int newLife)
    {
        life = newLife;
        playerHealth.UpdateHealthUI(life);
    }
}
