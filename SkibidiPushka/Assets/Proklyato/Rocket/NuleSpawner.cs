using UnityEngine;

public class NuleSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    public GameObject prefabToSpawn;
    public Vector2 spawnRangeX = new Vector2(10f, 50f);
    public Vector2 spawnRangeY = new Vector2(10f, 50f);

    public void Start()
    {
        EventManager.Instance.Subscribe("Atomic", SpawnRandomPrefab);
    }
    public void SpawnRandomPrefab()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Prefab to spawn is not assigned.");
            return;
        }

        float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
    }

}
