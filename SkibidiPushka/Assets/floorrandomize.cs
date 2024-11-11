using UnityEngine;

public class floorrandomize : MonoBehaviour
{
    public Sprite[] floorSprites;
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        float perlin = Mathf.PerlinNoise(transform.position.x * 100f, transform.position.y * 100f) + 1f;
        spriteRenderer.sprite = floorSprites[(int)(perlin * floorSprites.Length/2)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
