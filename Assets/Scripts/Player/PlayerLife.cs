using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int life;
    [SerializeField] private float repulsionForce = 10f; // A força de repulsão quando o jogador colide com um meteoro
    [SerializeField] private float damageFlashDuration = 0.1f; // Duração do efeito de flash de dano
    [SerializeField] private Color damageColor = Color.red; // Cor do efeito de dano
    [SerializeField] private GameObject playerVisual; // Referência ao objeto filho que contém o SpriteRenderer
    [SerializeField] private float scaleUpFactor = 1.2f; // Fator de aumento de escala ao tomar dano
    [SerializeField] private float scaleUpDuration = 0.1f; // Duração da animação de aumento de escala

    private Color originalColor;
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;

    public int Life
    {
        get { return life; }
        private set { life = value; }
    }

    private void Start()
    {
        if (playerVisual != null)
        {
            spriteRenderer = playerVisual.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
                originalScale = playerVisual.transform.localScale;
            }
            else
            {
                Debug.LogError("SpriteRenderer não encontrado no objeto playerVisual.");
            }
        }
        else
        {
            Debug.LogError("PlayerVisual não está atribuído.");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        life--;
        Debug.Log(life);

        // Efeito de dano visual
        if (spriteRenderer != null)
        {
            // Muda a cor de forma instantânea
            spriteRenderer.color = damageColor;
            DOTween.Sequence()
                .Append(playerVisual.transform.DOScale(originalScale * scaleUpFactor, scaleUpDuration))
                .Append(playerVisual.transform.DOScale(originalScale, scaleUpDuration))
                .Append(spriteRenderer.DOColor(originalColor, damageFlashDuration))
                .Play();
        }

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

        // Atualiza a UI da vida do jogador
        FindObjectOfType<PlayerHealth>().UpdateHealthUI(life);
    }
}
