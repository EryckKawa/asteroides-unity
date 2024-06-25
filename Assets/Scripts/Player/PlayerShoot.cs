using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform projectileSpawnPoint;
	[SerializeField] private float projectileSpeed = 20f;
	[SerializeField] private float shootCooldown = 0.5f; // Tempo mínimo entre os disparos
	[SerializeField] private float scaleIncrease = 5f; // Fator de aumento de escala dos projéteis

	private float shootTimer; // Timer para controlar o tempo entre os disparos
	private bool doubleShot = false; // Adicionado para o power-up de tiro duplo
	private bool isActiveIncreaseProjectile = false; // Flag para rastrear se o aumento de projétil está ativo

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
		// Se doubleShot é verdadeiro, cria um segundo projétil
		if (doubleShot)
		{
			Vector3 leftFirePoint = projectileSpawnPoint.position + projectileSpawnPoint.right * -0.5f;
			Vector3 rightFirePoint = projectileSpawnPoint.position + projectileSpawnPoint.right * 0.5f;
			CreateProjectile(leftFirePoint);
			CreateProjectile(rightFirePoint);
		}
		else
		{
			// Cria o projétil no ponto de spawn definido
			CreateProjectile(projectileSpawnPoint.position);
		}
	}

	private void CreateProjectile(Vector3 position)
	{
		GameObject projectile = Instantiate(projectilePrefab, position, projectileSpawnPoint.rotation);
		Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

		// Aplica aumento de escala se ActiveIncreaseProjectile estiver ativo
		if (isActiveIncreaseProjectile)
		{
			projectile.transform.localScale *= scaleIncrease;
		}

		Vector2 shootDirection = projectileSpawnPoint.up;
		rb.AddForce(shootDirection * projectileSpeed, ForceMode2D.Impulse);
	}

	public void ActivateDoubleShot(float duration)
	{
		StartCoroutine(DoubleShotRoutine(duration));
	}

	private IEnumerator DoubleShotRoutine(float duration)
	{
		doubleShot = true;
		yield return new WaitForSeconds(duration);
		doubleShot = false;
	}
	

	public void ActiveIncreaseProjectile(float duration)
	{
		if (!isActiveIncreaseProjectile)
		{
			isActiveIncreaseProjectile = true;
			StartCoroutine(IncreaseProjectileRoutine(duration));
		}
		else
		{
			// Caso já esteja ativo, reinicia a duração
			StopCoroutine(IncreaseProjectileRoutine(duration));
			StartCoroutine(IncreaseProjectileRoutine(duration));
		}
	}
	
	private IEnumerator IncreaseProjectileRoutine(float duration)
	{
		// Aumenta o tamanho dos projéteis durante a duração especificada
		yield return new WaitForSeconds(duration);

		// Restaura o tamanho original dos projéteis
		isActiveIncreaseProjectile = false; // Desativa o aumento de projétil
	}
	
	public bool GetActiveIncreaseProjectile()
	{
		return isActiveIncreaseProjectile;
	}
}
