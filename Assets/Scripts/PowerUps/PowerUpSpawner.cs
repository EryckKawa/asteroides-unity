using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs;
    [SerializeField] private float spawnRateStart = 5f;
    [SerializeField] private float spawnRateDecrease = 0.5f;
    [SerializeField] private float minSpawnRate = 1f;
    [SerializeField] private float forceTowardsPlayer = 5f;
    [SerializeField] private GameObject player;
    [SerializeField] private float startDelay = 10f; // Delay antes de começar a spawnar power-ups

    private float spawnRate;
    private Coroutine spawnCoroutine;
    private bool spawnSide = true;

    void Start()
    {
        spawnRate = spawnRateStart;
        StartCoroutine(StartSpawnWithDelay());
    }

    private IEnumerator StartSpawnWithDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnPowerUpsRoutine());
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnPowerUpsRoutine()
    {
        while (true)
        {
            if (spawnSide)
            {
                SpawnPowerUpSide();
            }
            else
            {
                SpawnPowerUpUpDown();
            }

            spawnSide = !spawnSide;
            spawnRate = Mathf.Max(spawnRate - spawnRateDecrease, minSpawnRate);

            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnPowerUpSide()
    {
        GameObject powerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        Vector2 spawnPosition = new Vector2(Mathf.Sign(Random.Range(-1f, 1f)) * 12, Random.Range(-5f, 5f));
        GameObject spawnedPowerUp = Instantiate(powerUp, spawnPosition, Quaternion.identity);
        ApplyForceTowardsPlayer(spawnedPowerUp);
    }

    private void SpawnPowerUpUpDown()
    {
        GameObject powerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        Vector2 spawnPosition = new Vector2(Random.Range(-10f, 10f), Mathf.Sign(Random.Range(-1f, 1f)) * 7);
        GameObject spawnedPowerUp = Instantiate(powerUp, spawnPosition, Quaternion.identity);
        ApplyForceTowardsPlayer(spawnedPowerUp);
    }

    private void ApplyForceTowardsPlayer(GameObject powerUp)
    {
        Rigidbody2D rb = powerUp.GetComponent<Rigidbody2D>();
        if (rb != null && player != null)
        {
            Vector2 direction = (player.transform.position - powerUp.transform.position).normalized;
            rb.AddForce(direction * forceTowardsPlayer, ForceMode2D.Impulse);

            float rotationForce = Random.Range(-100f, 100f);
            rb.AddTorque(rotationForce);
        }
    }
}
