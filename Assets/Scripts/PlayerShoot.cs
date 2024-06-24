using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float shootCooldown = 0.5f; // Tempo mínimo entre os disparos

    private float shootTimer; // Timer para controlar o tempo entre os disparos

    private void Update()
    {
        // Atualiza o timer
        shootTimer += Time.deltaTime;

        // Verifica se o jogador pode disparar novamente
        if (Input.GetMouseButtonDown(0) && shootTimer >= shootCooldown)
        {
            Shoot();
            shootTimer = 0f; // Reseta o timer após o disparo
        }
    }

    private void Shoot()
    {
        // Cria o projétil no ponto de spawn definido
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Obtém o componente Rigidbody2D do projétil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Calcula a direção do tiro baseado na rotação da nave
        Vector2 shootDirection = projectileSpawnPoint.up; // Assume-se que a nave está apontando para cima na hierarquia local

        // Aplica uma força ao projétil na direção do tiro
        rb.AddForce(shootDirection * projectileSpeed, ForceMode2D.Impulse);
    }
}
