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

    private Rigidbody2D rb;
    private bool isHeld = false;
    private Vector2 lastPosition;
    private float currentSpeed = 0f;

    private const string navMeshUpdateEvent = "UpdateNavMesh";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
    }

    public void GrabObject()
    {
        if (canBeGrabbed)
        {
            isHeld = true;
            gameObject.layer = LayerMask.NameToLayer(playerTouchedLayer);

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            EventManager.Instance.TriggerEvent(navMeshUpdateEvent);
        }
    }

    public void ReleaseObject()
    {
        if (isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;

            rb.velocity = (transform.position - (Vector3)lastPosition) / Time.deltaTime;

            EventManager.Instance.TriggerEvent(navMeshUpdateEvent);
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
        Destroy(gameObject);
    }
}
