using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectController : MonoBehaviour
{
    public string playerTouchedLayer = "PlayerTouched";
    public bool canBeGrabbed = false;

    [Header("Настройки объекта")]
    [SerializeField] private float mass = 5f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float minDamage = 5f;
    [SerializeField] private float maxDamage = 50f;
    [SerializeField] private float breakThreshold = 10f;
    [SerializeField] private float knockbackMultiplier = 0.5f;

    private float Maxhealth;

    private Rigidbody2D rb;
    private Collider2D objectCollider;
    private Renderer objectRenderer;
    private bool isHeld = false;
    private Vector2 lastPosition;
    private float currentSpeed = 0f;

    private float destroyDelay = 0.1f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();
        objectRenderer = GetComponent<Renderer>();

        rb.mass = mass;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Maxhealth = health;
    }

    public void GrabObject()
    {
        if (canBeGrabbed)
        {
            isHeld = true;
            gameObject.layer = LayerMask.NameToLayer(playerTouchedLayer);

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            EventManager.Instance.TriggerEvent("StartAttack");
        }
    }

    public void ReleaseObject()
    {
        if (isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;

            rb.velocity = (transform.position - (Vector3)lastPosition) / Time.deltaTime;

            EventManager.Instance.TriggerEvent("StopAttack");
        }
    }

    private void Update()
    {
        if (isHeld)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;

            currentSpeed = ((Vector2)transform.position - lastPosition).magnitude / Time.deltaTime;
            lastPosition = transform.position;
        }
        GetComponent<SpriteRenderer>().material.SetFloat("Force", 1f - health / Maxhealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHeld && collision.collider.CompareTag("Enemy"))
        {
            float damage = CalculateDamage();
            collision.collider.GetComponent<EnemyHealth>()?.TakeDamage(damage);

            ApplyKnockback(collision.collider, damage);
            TakeDamage(damage);
        }
    }

    private float CalculateDamage()
    {
        float damage = mass * currentSpeed * 0.1f;
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Объект получил урон: {damage}. Текущее здоровье: {health}");

        if (health <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        Debug.Log("Объект разрушен!");
        Invoke(nameof(DestroyAfterDelay), destroyDelay);
    }

    private void DestroyAfterDelay()
    {
        Destroy(gameObject);
    }
}
