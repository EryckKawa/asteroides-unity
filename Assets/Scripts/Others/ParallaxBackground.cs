using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform player; // Referência ao jogador
    [SerializeField] private float parallaxEffectMultiplier = 0.5f; // Multiplicador do efeito parallax

    private Vector3 lastPlayerPosition;

    private void Start()
    {
        lastPlayerPosition = player.position;
    }

    private void Update()
    {
        Vector3 deltaMovement = player.position - lastPlayerPosition;
        transform.position += deltaMovement * parallaxEffectMultiplier;
        lastPlayerPosition = player.position;
    }
}
