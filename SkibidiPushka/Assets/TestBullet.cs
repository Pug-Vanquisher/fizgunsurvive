using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    public float speed;
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }
    void Update()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * speed);
    }
}
