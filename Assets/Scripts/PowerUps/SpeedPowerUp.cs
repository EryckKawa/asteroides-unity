using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    [SerializeField] private float duration;
	private PlayerMovement playerMovement;

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if (other.CompareTag("Player"))
		{
			playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.ActiveSpeedBoost(duration);
		}
	}
}