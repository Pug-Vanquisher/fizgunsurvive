using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RocketController : MonoBehaviour
{
    public string playerTouchedLayer = "RaketkaTouched";
    public bool canBeGrabbed = true;

    [Header("Настройки ракеты")]
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float damage = 100f;
    [SerializeField] private GameObject explosionEffectPrefab;
    public float moveForce = 10f;

    private Rigidbody2D rb;
    private bool isHeld = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GrabObject()
    {
        if (canBeGrabbed)
        {
            isHeld = true;
            gameObject.layer = LayerMask.NameToLayer(playerTouchedLayer);

            //rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            EventManager.Instance.TriggerEvent("StartAttack");
        }
    }

    private void Update()
    {
        if (isHeld)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - rb.position).normalized;
            rb.AddForce(direction * moveForce);
        }
    }

    public void ReleaseObject()
    {
        if (isHeld)
        {
            isHeld = false;

            EventManager.Instance.TriggerEvent("StopAttack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isHeld) return;

        Explode();
    }

    private void Explode()
    {
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hitCollider in hitColliders)
        {
            int collidedLayer = hitCollider.gameObject.layer;

            if (collidedLayer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
                ApplyKnockback(hitCollider);
            }
            else if (collidedLayer == LayerMask.NameToLayer("Wall") || collidedLayer == LayerMask.NameToLayer("PlayerTouched"))
            {
                ObjectController objectController = hitCollider.GetComponent<ObjectController>();
                if (objectController != null)
                {
                    objectController.TakeDamage(damage);
                }
                ApplyKnockback(hitCollider);
            }
            else if (collidedLayer == LayerMask.NameToLayer("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
                ApplyKnockback(hitCollider);
            }
        }

        EventManager.Instance.TriggerEvent("StopAttack");

        Destroy(gameObject);
    }

    private void ApplyKnockback(Collider2D hitCollider)
    {
        Rigidbody2D hitRb = hitCollider.GetComponent<Rigidbody2D>();
        if (hitRb != null)
        {
            Vector2 knockbackDirection = (hitCollider.transform.position - transform.position).normalized;
            float knockbackForce = damage * 0.5f;
            hitRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
