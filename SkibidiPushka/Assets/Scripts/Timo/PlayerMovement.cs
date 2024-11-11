using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] float speed;
    [SerializeField] Animator animator;
    protected Vector3 direct;
    private bool _isNoFlipped = true;

    private void Start()
    {
        EventManager.Instance.Subscribe("StartAttack", Attack);
        EventManager.Instance.Subscribe("StopAttack", StopAttack);
    }
    void FixedUpdate()
    {
        direct.x = Input.GetAxis("Horizontal");
        direct.y = Input.GetAxis("Vertical");
        Move();
        Flip();

    }

    private void Move()
    {
        playerBody.velocity = direct.normalized * speed * Time.fixedDeltaTime * 50;
        animator.SetFloat("Velocity", playerBody.velocity.magnitude);
    }

    void Flip()
    {
        if (playerBody.velocity.x < 0 && _isNoFlipped)
        {
            playerBody.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _isNoFlipped = false;
        }
        else if (playerBody.velocity.x > 0 && !_isNoFlipped)
        {
            playerBody.transform.localRotation = Quaternion.Euler(0, 180, 0);
            _isNoFlipped = true;
        }
    }

    void Attack()
    {
        animator.SetFloat("Attack", 1);
    }

    void StopAttack()
    {
        animator.SetFloat("Attack", -1);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe("StartAttack", Attack);
        EventManager.Instance.Unsubscribe("StopAttack", StopAttack);
    }

}

