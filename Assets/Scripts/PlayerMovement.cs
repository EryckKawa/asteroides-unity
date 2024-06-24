using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float thrust = 50f; // Força de propulsão
    [SerializeField] private float rotationSpeed = 100f; // Velocidade de rotação

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ProcessThrust();
        PointTowardsMouse();
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(thrust * Time.deltaTime * Vector3.up);
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
}
