using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] ObjectsPrefabs;
    public float[] SummonChance;
    public Vector2 size;

    private float timer;
    private void Start()
    {
        GenerateRoom();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 5f)
        {
            timer = 0;
            EventManager.Instance.TriggerEvent("CorruptTiles");
        }
    }
    void GenerateRoom()
    {
        for (int y = 0; y < size.y+1; y++) { 
            for(int x = 0; x < size.x+1; x++)
            {
                if(x == 0 || y == 0 || x == size.x || y == size.y)
                {
                    Summon(1, new Vector2(x, y));
                }
                else
                {
                    if(Random.Range(0, 4) > 0)
                    {
                        Summon(2, new Vector2(x, y));
                    }
                    float rnd = Random.Range(0f, 1f) * 100;
                    for (int i = 0; i < SummonChance.Length; i++)
                    {
                        rnd -= SummonChance[i];
                        if(rnd <= 0)
                        {
                            Summon(i, new Vector2(x, y));
                            break;
                        }
                    }
                }
            }
        }
    }

    void Summon(int _id, Vector2 _position)
    {
        Instantiate(ObjectsPrefabs[_id], _position, Quaternion.identity);
    }

}
