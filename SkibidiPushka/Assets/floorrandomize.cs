using UnityEngine;

public class floorrandomize : MonoBehaviour
{
    public Sprite[] floorSprites;
    public SpriteRenderer spriteRenderer;
    public float perlin;
    public Vector3 trans;
    void Start()
    {
        trans = transform.position;
        perlin = Mathf.PerlinNoise(transform.position.x * 100f, transform.position.y * 100f) + 1f;
        spriteRenderer.sprite = floorSprites[(int)(perlin * floorSprites.Length / 2)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
