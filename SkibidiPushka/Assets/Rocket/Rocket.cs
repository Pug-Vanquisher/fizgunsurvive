using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float damage = 100f;
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float bulletLifetime = 5f;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float accelerationTime = 3f; 


    private Coroutine lifeTimeCoroutine;
    private Rigidbody2D rb;
    private float initialSpeed;
    private float accelerationRate;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialSpeed = rb.velocity.magnitude; 
        accelerationRate = (maxSpeed - initialSpeed) / accelerationTime; 
        lifeTimeCoroutine = StartCoroutine(RocketLifeTime());
    }

    private void Update()
    {
        AccelerateRocket();
    }

    private void AccelerateRocket()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            float newSpeed = rb.velocity.magnitude + accelerationRate * Time.deltaTime;
            newSpeed = Mathf.Min(newSpeed, maxSpeed); 
            rb.velocity = rb.velocity.normalized * newSpeed;
        }
    }

    private IEnumerator RocketLifeTime()
    {
        yield return new WaitForSeconds(bulletLifetime);
        Explode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int collidedLayer = collision.gameObject.layer;

        if (collidedLayer == LayerMask.NameToLayer("Wall") ||
            collidedLayer == LayerMask.NameToLayer("PlayerTouched") ||
            collidedLayer == LayerMask.NameToLayer("Player"))
        {
            Explode();
        }
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
                continue;
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
