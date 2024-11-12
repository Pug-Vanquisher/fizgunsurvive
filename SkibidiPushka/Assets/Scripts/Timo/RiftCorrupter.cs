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
    public float FadeOut;

    private float Brightness;
    private float Force;


    void Start()
    {
        StartCoroutine(RiftInvasion());
        Brightness = WarpSprite.material.GetFloat("_Brightness");
        Force = NewEnemy.material.GetFloat("_Force");
    }
    private void Update()
    {
        transform.position = tiled(localPos + new Vector2(transform.parent.position.x, transform.parent.position.y));
    }

    IEnumerator RiftInvasion()
    {
        float fading = 0.52f / FadeIn;
        while (FadeIn > 0)
        {
            WarpSprite.material.SetFloat("_Brightness", Brightness + fading);
            FadeIn -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        } 

        fading = - 0.52f / FadeOut;
        float forcing = 1 / FadeOut;
        while (FadeOut > 0)
        {
            WarpSprite.material.SetFloat("_Brightness", Brightness + fading);
            NewEnemy.material.SetFloat("_Force", Force + forcing);
            FadeOut -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        KillTile();
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
        if(!collidersIn.ContainsKey(collision) && collision.gameObject.tag == "Enemy" && collision.gameObject.tag == "Player")
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
