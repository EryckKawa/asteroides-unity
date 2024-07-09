using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Desativa o sprite
            spriteRenderer.enabled = false;

            // Toca o som do power-up
            audioSource.Play();

            // Destrói o objeto após a duração do som
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
