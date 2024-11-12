using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrupterPlacer : MonoBehaviour
{
    public GameObject CorrupterPrefab;
    public GameObject[] ObjectsPrefabs;
    public int enemySpawnrate;
    public int obstalceSpawnrate;
    public float respawnMaxDistance;
    public float respawnMinDistance;
    void Start()
    {
        EventManager.Instance.Subscribe("CorruptTiles", StartTileRemake);

        EventManager.Instance.Subscribe("HighActivity", ActiviyUpscale);
        EventManager.Instance.Subscribe("HighLoad", LoadUpscale);
    }

    void ActiviyUpscale()
    {
        enemySpawnrate += 2;
    }
    void LoadUpscale()
    {
        obstalceSpawnrate += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawnrate + obstalceSpawnrate != transform.childCount)
        {
            for (int i = 0; i < Mathf.Abs(enemySpawnrate + obstalceSpawnrate - transform.childCount); i++)
            {
                if (transform.childCount > enemySpawnrate + obstalceSpawnrate)
                {
                    Destroy(transform.GetChild(transform.childCount - i - 1).gameObject);
                }
                else if (transform.childCount < enemySpawnrate + obstalceSpawnrate)
                {
                    var a = Instantiate(CorrupterPrefab, transform);
                    a.GetComponent<EnvCorrupter>().localPos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(respawnMinDistance, respawnMaxDistance);
                }
            }
        }
    }
    void StartTileRemake()
    {
        Invoke("TileRemake", 0.1f);
    }
    void TileRemake()
    {
        int enemies = enemySpawnrate;
        int obstacles = obstalceSpawnrate;

        while(enemies + obstacles > 0)
        {
            int rnd = Random.Range(0, enemies + obstacles);
            if(rnd < enemies)
            {
                Summon(0, transform.GetChild(enemies + obstacles-1).transform.position);
                enemies -= 1;
            }
            else if(rnd >= enemies)
            {
                Summon(Random.Range(1, ObjectsPrefabs.Length), transform.GetChild(enemies + obstacles-1).transform.position);
                obstacles -= 1;
            }
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<EnvCorrupter>().localPos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(respawnMinDistance, respawnMaxDistance);
        }

        EventManager.Instance.TriggerEvent("UpdateNavMesh");
    }



    void Summon(int _id, Vector2 _position)
    {
        Instantiate(ObjectsPrefabs[_id], _position, Quaternion.identity);
    }

}
