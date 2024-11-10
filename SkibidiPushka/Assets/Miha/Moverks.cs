using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moverks : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� �����������
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �������� ����������� �������� �� ���� X � Y
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        // ����������� ������, ����� �������� �� ���� ������� �� ���������
        moveDirection.Normalize();
    }

    void FixedUpdate()
    {
        // ��������� �������� � Rigidbody2D
        rb.velocity = moveDirection * moveSpeed;
    }
}