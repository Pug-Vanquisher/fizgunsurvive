using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] float speed;
    protected Vector3 direct;
    void Update()
    {
        direct.x = Input.GetAxis("Horizontal");
        direct.y = Input.GetAxis("Vertical");
        Move(direct.normalized);
        
    }

    private void Move(Vector3 direct)
    {
        playerBody.velocity = direct * speed;
    }

   
}

