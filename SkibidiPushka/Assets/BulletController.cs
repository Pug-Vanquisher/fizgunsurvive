using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
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

    private Rigidbody2D rb;
    private Collider2D objectCollider;
    private Renderer objectRenderer;
    private bool isHeld = false;
    private Vector2 lastPosition;
    private float currentSpeed = 0f;

    private float destroyDelay = 0.1f; // Задержка перед уничтожением объекта

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();
        objectRenderer = GetComponent<Renderer>();

        rb.mass = mass;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void GrabObject()
    {
        if (canBeGrabbed)
        {
            isHeld = true;
            gameObject.layer = LayerMask.NameToLayer(playerTouchedLayer);

            EventManager.Instance.TriggerEvent("StartAttack");

            //rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
    }

    public void ReleaseObject()
    {
        if (isHeld)
        {
            isHeld = false;
            //rb.isKinematic = false;

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isHeld)
        {
            return;
        }

        int collidedLayer = collision.gameObject.layer;
        float damage = CalculateDamage();

        if (isHeld && collidedLayer == LayerMask.NameToLayer("Enemy"))
        {
            collision.collider.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            ApplyKnockback(collision.collider, damage);
        }
        else if (isHeld && collidedLayer == LayerMask.NameToLayer("Wall") || collidedLayer == LayerMask.NameToLayer("PlayerTouched"))
        {
            // урон через ObjectController
            ObjectController objectController = collision.gameObject.GetComponent<ObjectController>();
            if (objectController != null)
            {
                objectController.TakeDamage(damage);
            }
        }

        // Пуля разрушает себя после столкновения
        TakeDamage(damage);
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
        Debug.Log($"пуля получил урон: {damage}. Текущее здоровье: {health}");

        if (health <= 0)
        {
            EventManager.Instance.TriggerEvent("StopAttack");

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
