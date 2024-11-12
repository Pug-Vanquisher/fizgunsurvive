using UnityEngine;
using System.Collections;

public class WildRocket : MonoBehaviour
{
    [Header("Настройки ракеты")]
    [SerializeField] private float damage = 100f;
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float rocketLifetime = 5f;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private float initialSpeed = 5f;
    [SerializeField] private float maxSpeed = 20f;

    private float currentSpeed;
    private float lifetimeElapsed;
    private Vector2 currentDirection;
    private Coroutine lifeTimeCoroutine;

    private void Start()
    {
        currentDirection = transform.right;
        currentSpeed = initialSpeed;
        lifeTimeCoroutine = StartCoroutine(RocketLifeTime());
    }

    private void Update()
    {
        lifetimeElapsed += Time.deltaTime;

        currentSpeed = Mathf.Lerp(initialSpeed, maxSpeed, lifetimeElapsed / rocketLifetime);

        transform.Translate(currentDirection * currentSpeed * Time.deltaTime, Space.World);
    }

    private IEnumerator RocketLifeTime()
    {
        yield return new WaitForSeconds(rocketLifetime);
        Explode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int collidedLayer = collision.gameObject.layer;

        if (collidedLayer == LayerMask.NameToLayer("Player") ||
            collidedLayer == LayerMask.NameToLayer("Enemy"))
        {
            Explode();
        }
        else
        {
            Vector2 collisionNormal = collision.contacts[0].normal;
            currentDirection = Vector2.Reflect(currentDirection, collisionNormal).normalized;

            RotateRocket();
        }
    }

    private void RotateRocket()
    {
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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

    public void CancelLifeTimeCoroutine()
    {
        if (lifeTimeCoroutine != null)
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null;
        }
    }
}
