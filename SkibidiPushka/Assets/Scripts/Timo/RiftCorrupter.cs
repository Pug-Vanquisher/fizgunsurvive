using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftCorrupter : MonoBehaviour
{
    private Dictionary<Collider2D, GameObject> collidersIn = new Dictionary<Collider2D, GameObject>();
    public Vector2 localPos;
    void Start()
    {
        EventManager.Instance.Subscribe("CorruptTiles", KillTile);
    }
    private void Update()
    {
        transform.position = tiled(localPos + new Vector2(transform.parent.position.x, transform.parent.position.y));
    }
    void KillTile()
    {
        List<GameObject> gmbj = new List<GameObject>();
        foreach (KeyValuePair<Collider2D, GameObject> pair in collidersIn)
        {
            gmbj.Add(pair.Value);
        }
        foreach (GameObject gm in gmbj)
        {
            Destroy(gm);
        }
        collidersIn.Clear();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collidersIn.ContainsKey(collision))
        {
            collidersIn.Add(collision, collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidersIn.Remove(collision);
    }
    Vector2 tiled(Vector2 pos)
    {
        return new Vector2((int)pos.x, (int)pos.y);
    }
}
