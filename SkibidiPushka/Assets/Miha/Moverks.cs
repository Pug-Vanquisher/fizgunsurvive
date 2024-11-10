using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moverks : MonoBehaviour
{
    public float moveSpeed = 5f; // —корость перемещени€
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ѕолучаем направление движени€ по ос€м X и Y
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        // Ќормализуем вектор, чтобы движение не было быстрее по диагонали
        moveDirection.Normalize();
    }

    void FixedUpdate()
    {
        // ѕримен€ем скорость к Rigidbody2D
        rb.velocity = moveDirection * moveSpeed;
    }
}