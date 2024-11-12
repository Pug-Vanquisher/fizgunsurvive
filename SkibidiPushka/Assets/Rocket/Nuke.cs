using UnityEngine;

public class Nuke : MonoBehaviour
{

    [SerializeField] private float damage = 200f;
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private GameObject explosionEffectPrefab;

    void Start()
    {
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
