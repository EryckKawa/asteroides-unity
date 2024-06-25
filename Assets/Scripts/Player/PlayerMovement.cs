using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float thrust = 50f; // Força de propulsão normal
	[SerializeField] private float rotationSpeed = 100f; // Velocidade de rotação
	[SerializeField] private float brakeForce = 30f; // Força de freio
	[SerializeField] private float speedBoostMultiplier = 2f; // Multiplicador de aumento de velocidade
	[SerializeField] private GameObject rocketPropulsion;

	private Rigidbody2D rb;
	private float originalThrust; // Força de propulsão original do jogador
	private bool isActiveSpeedBoost = false; // Flag para rastrear se o aumento de velocidade está ativo

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		originalThrust = thrust; // Armazena a força de propulsão original
	}

	private void Update()
	{
		ProcessThrust();
		ProcessBrake(); // Processa o freio
		PointTowardsMouse();
	}

	private void ProcessThrust()
	{
		float currentThrust = isActiveSpeedBoost ? thrust * speedBoostMultiplier : thrust;

		if (Input.GetKey(KeyCode.Space))
		{
			rb.AddRelativeForce(currentThrust * Time.deltaTime * Vector3.up);
			rocketPropulsion.SetActive(true);
		}
		else
		{
			rocketPropulsion.SetActive(false);
		}
	}

	private void ProcessBrake()
	{
		if (Input.GetMouseButton(1)) // Botão direito do mouse (0 para esquerdo, 1 para direito, 2 para meio)
		{
			// Aplica uma força oposta à direção atual do movimento para desacelerar
			rb.AddForce(-rb.velocity.normalized * brakeForce * Time.deltaTime, ForceMode2D.Force);
		}
	}

	private void PointTowardsMouse()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0f; // Resetar a posição Z

		Vector3 direction = mousePosition - transform.position;
		float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
		float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

		rb.freezeRotation = true; // Congela a rotação manualmente
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		rb.freezeRotation = false; // Descongela a rotação
	}

	public void ActiveSpeedBoost(float duration)
	{
		if (!isActiveSpeedBoost)
		{
			isActiveSpeedBoost = true;
			thrust *= speedBoostMultiplier; // Aumenta a força de propulsão
			StartCoroutine(SpeedBoostRoutine(duration));
		}
		else
		{
			StopCoroutine(SpeedBoostRoutine(duration));
			StartCoroutine(SpeedBoostRoutine(duration));
		}
	}

	private IEnumerator SpeedBoostRoutine(float duration)
	{
		yield return new WaitForSeconds(duration);
		thrust = originalThrust; // Restaura a força de propulsão original
		isActiveSpeedBoost = false; // Desativa o aumento de velocidade
	}
}
