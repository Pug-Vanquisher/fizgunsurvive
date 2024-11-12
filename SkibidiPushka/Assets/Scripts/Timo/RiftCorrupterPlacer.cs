using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftCorrupterPlacer : MonoBehaviour
{
    public GameObject CorrupterPrefab;
    public GameObject[] bulletsPrefabs;
    public int enemySpawnrate;
    public int obstalceSpawnrate;
    public float respawnMaxDistance;
    public float respawnMinDistance;
    void Start()
    {
        EventManager.Instance.Subscribe("CorruptTiles", TileRemake);
    }
    void TileRemake()
    {
        for (int i = 0; i < Mathf.Abs(enemySpawnrate + obstalceSpawnrate - transform.childCount); i++)
        {
            var a = Instantiate(CorrupterPrefab, transform);
            a.GetComponent<RiftCorrupter>().localPos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(respawnMinDistance, respawnMaxDistance);
        }
        EventManager.Instance.TriggerEvent("UpdateNavMesh");
    }
}
