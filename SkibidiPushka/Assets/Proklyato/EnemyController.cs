using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public bool canBeGrabbed = true;
    public float massForDamageCalculation = 5f;
    public float minDamage = 5f;
    public float maxDamage = 50f;
    public float knockbackMultiplier = 0.5f;
    public float moveForce = 10f; 

    private float currentSpeed = 0f;
    private Rigidbody2D rb;
    private EnemyAI enemyAI;
    private EnemyHealth enemyHealth;
    private bool isHeld = false;
    private Vector2 lastPosition;

    public bool IsHeld => isHeld; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAI = GetComponent<EnemyAI>();
        enemyHealth = GetComponent<EnemyHealth>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void GrabObject()
    {
        if (canBeGrabbed)
        {
            isHeld = true;
            enemyAI.SetAgentEnabled(false);
            //rb.isKinematic = false;
        }
    }

    public void ReleaseObject()
    {
        if (isHeld)
        {
            isHeld = false;
            enemyAI.SetAgentEnabled(true);
        }
    }

    private void Update()
    {
        if (isHeld)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - rb.position).normalized;
            rb.AddForce(direction * moveForce);

            currentSpeed = direction.magnitude / Time.deltaTime;
            lastPosition = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHeld && collision.collider.CompareTag("Enemy"))
        {
            float damage = CalculateDamage();
            collision.collider.GetComponent<EnemyHealth>()?.TakeDamage(damage);

            ApplyKnockback(collision.collider, damage);
            enemyHealth.TakeDamage(damage);
        }
    }

    private float CalculateDamage()
    {
        float damage = massForDamageCalculation * currentSpeed * 0.01f;
        return Mathf.Clamp(damage, minDamage, maxDamage);
    }

    private void ApplyKnockback(Collider2D enemy, float damage)
    {
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            float knockbackForce = damage * knockbackMultiplier;
            enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
