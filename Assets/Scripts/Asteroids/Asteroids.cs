using UnityEngine;

public class Asteroids : MonoBehaviour
{
	[SerializeField] private int health; // Vida inicial do asteroide
	[SerializeField] private ParticleSystem destructionParticle;

	private void OnTriggerEnter2D(Collider2D other)
	{
		health--;
		Debug.Log("Asteroid hit. Health: " + health);
		Destroy(other.gameObject);

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
			Instantiate(destructionParticle, transform.position, transform.rotation);
			Destroy(gameObject);
			Debug.Log("Asteroid destroyed.");
		}
	}
}
