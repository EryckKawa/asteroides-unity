using System.Collections;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private float spawnRateStart = 3f;
    [SerializeField] private float spawnRateDecrease = 0.01f;
    [SerializeField] private float minSpawnRate = 1f;
    [SerializeField] private float forceTowardsPlayer = 5f;
    [SerializeField] private GameObject player;

    private float spawnRate;
    private Coroutine spawnCoroutine;
    private bool spawnSide = true; // Flag para alternar entre spawns laterais e up/down

    void Start()
    {
        spawnRate = spawnRateStart;
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnAsteroidsRoutine());
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

    private IEnumerator SpawnAsteroidsRoutine()
    {
        while (true)
        {
            // Spawn de acordo com a flag spawnSide
            if (spawnSide)
            {
                SpawnAsteroidSide();
            }
            else
            {
                SpawnAsteroidUpDown();
            }

            spawnSide = !spawnSide; // Alternar entre side e up/down

            // Diminui o spawnRate até o mínimo definido
            spawnRate = Mathf.Max(spawnRate - spawnRateDecrease, minSpawnRate);

            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnAsteroidSide()
    {
        GameObject asteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
        Vector2 spawnPosition = new Vector2(Mathf.Sign(Random.Range(-1f, 1f)) * 12, Random.Range(-5f, 5f));
        GameObject spawnedAsteroid = Instantiate(asteroid, spawnPosition, Quaternion.identity);
        ApplyForceTowardsPlayer(spawnedAsteroid);
    }

    private void SpawnAsteroidUpDown()
    {
        GameObject asteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
        Vector2 spawnPosition = new Vector2(Random.Range(-10f, 10f), Mathf.Sign(Random.Range(-1f, 1f)) * 7);
        GameObject spawnedAsteroid = Instantiate(asteroid, spawnPosition, Quaternion.identity);
        ApplyForceTowardsPlayer(spawnedAsteroid);
    }

    private void ApplyForceTowardsPlayer(GameObject asteroid)
    {
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null && player != null)
        {
            Vector2 direction = (player.transform.position - asteroid.transform.position).normalized;
            rb.AddForce(direction * forceTowardsPlayer, ForceMode2D.Impulse);

            // Adiciona uma rotação aleatória
            float rotationForce = Random.Range(-100f, 100f);
            rb.AddTorque(rotationForce);
        }
    }
}
