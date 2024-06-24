using UnityEngine;

public class Asteroids : MonoBehaviour
{
	[SerializeField] private int health; // Vida inicial do asteroide

	void OnTriggerEnter2D(Collider2D other)
	{
		// Reduz a saúde quando colide com outro objeto
		health--;
		Destroy(other.gameObject);

		// Verifica se a saúde chegou a zero para destruir o asteroide
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}
}
