using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int life;
    [SerializeField] private float repulsionForce = 10f; // A força de repulsão quando o jogador colide com um meteoro
    [SerializeField] private GameObject gameOverScreen; // Referência ao objeto gameOverScreen no Canvas
    private SpriteRenderer playerVisual;
    private PlayerHealth playerHealth;

    private void Start()
    {
        // Inicializa a referência ao SpriteRenderer do playerVisual
        playerVisual = transform.Find("PlayerVisual").GetComponent<SpriteRenderer>();

        // Encontra o componente PlayerHealth no mesmo GameObject ou em um GameObject associado
        playerHealth = GetComponent<PlayerHealth>();
    }

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

        // Animação de dano
        AnimateDamage();

        // Atualiza a UI de vida
        playerHealth.UpdateHealthUI(life);

        // Verifica se a vida é 0 ou menor
        if (life <= 0)
        {
            ShowGameOverScreen();
        }
    }

    void AnimateDamage()
    {
        // Animação de cor e escala para indicar dano
        playerVisual.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            playerVisual.DOColor(Color.white, 0.1f);
        });
        playerVisual.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.3f, 10, 1);
    }

    void ShowGameOverScreen()
    {
        // Mostra a tela de Game Over
        gameOverScreen.SetActive(true);

        // Pausa o jogo
        Time.timeScale = 0f;

        // Para a música
        FindObjectOfType<AudioManager>().StopMusic();
    }

}
