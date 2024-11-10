using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] Animator animator;

    private Vector2 vel;
    
    // Start is called before the first frame update
    void Start()
    {
        if (rb != null) { rb = GetComponent<Rigidbody2D>(); }
    }

    // Update is called once per frame
    void Update()
    {
        vel.x = Input.GetAxisRaw("Horizontal");
        vel.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        animator.SetFloat("speed", Mathf.Abs(vel.x) + Mathf.Abs(vel.y));
        if (vel.x > 0) { animator.SetBool("going_right", true); }
        else { animator.SetBool("going_right", false); }
        Move(rb,vel,speed);
    }

    public void Move(Rigidbody2D rb, Vector2 vel, float speed)
    {
        rb.velocity = vel * speed * Time.deltaTime;
    }
}
