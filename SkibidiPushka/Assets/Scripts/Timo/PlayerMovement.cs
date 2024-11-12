using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] float speed;
    [SerializeField] Animator animator;
    [SerializeField] KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] float dashMulti;
    [SerializeField] float dashTime;
    private float _currDashTime;
    protected Vector3 direct;
    private bool _isNoFlipped = true;
    private WaitForFixedUpdate _waiter = new WaitForFixedUpdate();

    private void Start()
    {
        EventManager.Instance.Subscribe("StartAttack", Attack);
        EventManager.Instance.Subscribe("StopAttack", StopAttack);
    }
    void Update()
    {
        direct.x = Input.GetAxis("Horizontal");
        direct.y = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(dashKey))
        {
            Debug.Log("sdfjdjklhsfgsjhfg;lsdg");
            _currDashTime = 0;
            StartCoroutine(Dash(direct.normalized));
        }
        else { Move(); }
        

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

    private IEnumerator Dash(Vector3 direct)
    {
        while (_currDashTime < dashTime)
        {
            _currDashTime += Time.deltaTime;
            playerBody.velocity = direct * speed * dashMulti * Time.fixedDeltaTime * 50;
            yield return _waiter;
        }
        yield return null;
    }

}

