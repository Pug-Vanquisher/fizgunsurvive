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
    void Update()
    {
        direct.x = Input.GetAxis("Horizontal");
        direct.y = Input.GetAxis("Vertical");
        Move();

    }

    private void Move()
    {
        playerBody.velocity = direct.normalized * speed * Time.fixedDeltaTime * 50;
        animator.SetFloat("Speed", playerBody.velocity.magnitude/speed);
        animator.SetFloat("X", playerBody.velocity.normalized.x);
    }

    void Attack()
    {
        animator.SetFloat("Attack", 1);
    }

    void StopAttack()
    {
        animator.SetFloat("Attack", 0);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe("StartAttack", Attack);
        EventManager.Instance.Unsubscribe("StopAttack", StopAttack);
    }

}

