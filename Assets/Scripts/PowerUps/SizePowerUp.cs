using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePowerUp : PowerUp
{
    [SerializeField] private float duration;
	private PlayerShoot playerShoot;

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if (other.CompareTag("Player"))
		{
			playerShoot = other.GetComponent<PlayerShoot>();
			playerShoot.ActiveIncreaseProjectile(duration);

		}
	}
}
