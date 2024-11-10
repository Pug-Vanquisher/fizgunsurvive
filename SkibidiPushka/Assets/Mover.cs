using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move(Rigidbody2D rb, Vector2 velik, float speed)
    {
        rb.velocity = velik.normalized * speed;
    }
}
