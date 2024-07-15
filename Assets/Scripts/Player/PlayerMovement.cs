using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float thrust = 50f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float brakeForce = 30f;
    [SerializeField] private float speedBoostMultiplier = 2f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private GameObject rocketPropulsion;

    private Rigidbody2D rb;
    private PlayerInputActions inputActions;
    private bool isActiveSpeedBoost = false;
    private bool isThrusting;
    private bool isBraking;
    private Vector2 gamepadDirection;
    private Vector2 lastGamepadDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => isThrusting = true;
        inputActions.Player.Move.canceled += ctx => isThrusting = false;
        inputActions.Player.Stop.performed += ctx => isBraking = true;
        inputActions.Player.Stop.canceled += ctx => isBraking = false;
        inputActions.Player.Look.performed += ctx => gamepadDirection = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => gamepadDirection = Vector2.zero;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= ctx => isThrusting = true;
        inputActions.Player.Move.canceled -= ctx => isThrusting = false;
        inputActions.Player.Stop.performed -= ctx => isBraking = true;
        inputActions.Player.Stop.canceled -= ctx => isBraking = false;
        inputActions.Player.Look.performed -= ctx => gamepadDirection = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled -= ctx => gamepadDirection = Vector2.zero;
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessBrake();
    }

    private void Update()
    {
        if (gamepadDirection != Vector2.zero)
        {
            lastGamepadDirection = gamepadDirection;
            PointTowardsGamePadLeftStick();
        }
        else if (lastGamepadDirection != Vector2.zero)
        {
            PointTowardsLastGamePadDirection();
        }
        else
        {
            PointTowardsMouse();
        }
    }

    private void ProcessThrust()
    {
        float currentThrust = isActiveSpeedBoost ? thrust * speedBoostMultiplier : thrust;
        Vector2 force = currentThrust * Time.deltaTime * transform.up;

        if (isThrusting)
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
        if (isBraking)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, brakeForce * Time.deltaTime);
        }
    }

    private void PointTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0f;

        Vector3 direction = mousePosition - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rb.freezeRotation = false;
    }

    private void PointTowardsGamePadLeftStick()
    {
        float targetAngle = Mathf.Atan2(gamepadDirection.y, gamepadDirection.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rb.freezeRotation = false;
    }

    private void PointTowardsLastGamePadDirection()
    {
        float targetAngle = Mathf.Atan2(lastGamepadDirection.y, lastGamepadDirection.x) * Mathf.Rad2Deg - 90f;
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
        thrust = thrust;
        isActiveSpeedBoost = false;
    }
}
