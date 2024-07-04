using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float thrust = 50f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float brakeForce = 30f;
    [SerializeField] private float speedBoostMultiplier = 2f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private GameObject rocketPropulsion;

    private Rigidbody2D rb;
    private float originalThrust;
    private bool isActiveSpeedBoost = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        originalThrust = thrust;
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessBrake();
    }

    private void Update()
    {
        PointTowardsMouse();
    }

    private void ProcessThrust()
    {
        float currentThrust = isActiveSpeedBoost ? thrust * speedBoostMultiplier : thrust;
        Vector2 force = currentThrust * Time.deltaTime * transform.up;

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(force);
            rocketPropulsion.SetActive(true);
        }
        else
        {
            rocketPropulsion.SetActive(false);
        }

        // Limitar a velocidade máxima
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void ProcessBrake()
    {
        if (Input.GetMouseButton(1))
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, brakeForce * Time.deltaTime);
        }
    }

    private void PointTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = mousePosition - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rb.freezeRotation = false;
    }

    public void ActivateSpeedBoost(float duration)
    {
        if (!isActiveSpeedBoost)
        {
            isActiveSpeedBoost = true;
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
        thrust *= speedBoostMultiplier;
        yield return new WaitForSeconds(duration);
        thrust = originalThrust;
        isActiveSpeedBoost = false;
    }
}
