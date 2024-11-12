using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftCorrupter : MonoBehaviour
{
    private Dictionary<Collider2D, GameObject> collidersIn = new Dictionary<Collider2D, GameObject>();
    public Vector2 localPos;
    public GameObject EnemyPrefab;
    public SpriteRenderer WarpSprite;
    public SpriteRenderer NewEnemy;

    public float FadeIn;
    public float timer;

    private float Brightness;
    private float Force;


    void Start()
    {
        Brightness = WarpSprite.material.GetFloat("_Brightness");
        Force = NewEnemy.material.GetFloat("_Force");
    }
    private void Update()
    {
        transform.position = tiled(localPos + new Vector2(transform.parent.position.x, transform.parent.position.y));
        timer += Time.deltaTime;
        if (timer < FadeIn / 2)
        {
            Brightness = Mathf.Lerp(Brightness, -0.2f, 0.01f);
            Force = 10;
        }
        else if (timer >= FadeIn / 2  && timer < FadeIn)
        {
            Brightness = Mathf.Lerp(Brightness, 5, 0.01f);
            Force = Mathf.Lerp(Force, 0, 0.1f);
        }
        else if (timer >= FadeIn)
        {
            KillTile();
        }
        WarpSprite.material.SetFloat("_Brightness", Brightness);
        NewEnemy.material.SetFloat("_Force", Force);
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
        Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collidersIn.ContainsKey(collision) && collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Player" && collision.gameObject.tag != "WallBuilder")
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
