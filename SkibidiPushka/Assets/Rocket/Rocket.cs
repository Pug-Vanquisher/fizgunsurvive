using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float damage = 100f;
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float bulletLifetime = 5f;
    [SerializeField] private GameObject explosionEffectPrefab;


    private Coroutine lifeTimeCoroutine;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeTimeCoroutine = StartCoroutine(RocketLifeTime());
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
            }
            else if (collidedLayer == LayerMask.NameToLayer("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
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
