using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class GranadeController : MonoBehaviour
{
    public string granadeTouchedLayer = "GranadeTouched";
    public bool canBeGrabbed = false;
    public float explosionDelay = 3f; 
    public float damage = 100f; 
    public GameObject explosionEffectPrefab; 
    public TMP_Text countdownText; 

    private Rigidbody2D rb;
    private bool isHeld = false;
    private bool isExploding = false;
    private float explosionTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        if (isHeld)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;

            explosionTimer -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(explosionTimer).ToString();

            if (explosionTimer <= 0 && !isExploding)
            {
                StartCoroutine(Explode());
            }
        }
    }

    public void GrabObject()
    {
        if (canBeGrabbed)
        {
            isHeld = true;
            gameObject.layer = LayerMask.NameToLayer(granadeTouchedLayer);
            explosionTimer = explosionDelay;

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void ReleaseObject()
    {
        if (isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
        }
    }

    private IEnumerator Explode()
    {
        isExploding = true;

        // эффект взрыва
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        // Найти все объекты в радиусе взрыва
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 4f); // Радиус взрыва

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

        Destroy(gameObject);

        yield return null;
    }

    private void ApplyKnockback(Collider2D hitCollider)
    {
        Rigidbody2D hitRb = hitCollider.GetComponent<Rigidbody2D>();
        if (hitRb != null)
        {
            Vector2 knockbackDirection = (hitCollider.transform.position - transform.position).normalized;
            float knockbackForce = damage * 0.1f; // Сила отталкивания
            hitRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
