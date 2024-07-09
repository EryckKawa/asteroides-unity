using System.Collections;
using TMPro;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    [SerializeField] private int health; // Vida inicial do asteroide
    [SerializeField] private int scorePoints; // Pontos que o asteroide dá ao ser destruído
    [SerializeField] private ParticleSystem destructionParticle; // Sistema de partículas para destruição
    [SerializeField] private ParticleSystem asteroidsParticle; // Sistema de partículas do asteroide
    [SerializeField] private GameObject scoreTextPrefab; // Prefab para o texto de pontuação
    [SerializeField] private AudioClip explosionSound; // Som de explosão
    [SerializeField] private AudioClip hitSound; // Som de impacto
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private AudioSource audioSource;

    private void Start()
    {
        // Inicializa o SpriteRenderer, o CircleCollider2D e o AudioSource
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        health--;
        Debug.Log("Asteroid hit. Health: " + health);
        Destroy(other.gameObject);

        // Toca o som de impacto
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        BaseProjectile projectile = other.GetComponent<BaseProjectile>();
        if (projectile != null)
        {
            if (projectile.GetIsSizeIncreased())
            {
                health -= 2;
                Debug.Log("Projectile was increased. Health: " + health);
            }
        }

        // Verifica se a saúde chegou a zero para destruir o asteroide
        if (health <= 0)
        {
            if (destructionParticle != null)
            {
                Instantiate(destructionParticle, transform.position, transform.rotation).Play();
            }

            if (asteroidsParticle != null)
            {
                asteroidsParticle.Stop();
            }

            ShowScoreText(scorePoints); // Exibir texto de pontuação

            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(scorePoints); // Adicionar pontos ao ScoreManager
            }

            Debug.Log("Asteroid destroyed.");

            StartCoroutine(DisableAndDestroy(2.0f)); // Desativar e destruir asteroide após um atraso
        }
    }

    private void ShowScoreText(int points)
    {
        scoreTextPrefab.SetActive(true);
        TextMeshProUGUI textMesh = scoreTextPrefab.GetComponent<TextMeshProUGUI>();
        textMesh.text = "+" + points;
        StartCoroutine(DisplayAndHide(scoreTextPrefab, 2.0f)); // Exibir texto por 2 segundos
    }

    private IEnumerator DisplayAndHide(GameObject obj, float delay)
    {
        obj.SetActive(true); // Ativar objeto
        yield return new WaitForSeconds(delay); // Esperar 2 segundos
        obj.SetActive(false); // Desativar objeto
    }

    private IEnumerator DisableAndDestroy(float delay)
    {
        // Toca o som de explosão
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Desativa o SpriteRenderer, o CircleCollider2D e os sistemas de partículas
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;

        if (asteroidsParticle != null)
        {
            asteroidsParticle.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(delay); // Esperar 2 segundos

        Destroy(gameObject); // Destruir asteroide
    }
}
