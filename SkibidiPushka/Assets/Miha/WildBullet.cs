using UnityEngine;
using System.Collections;

public class WildBullet : MonoBehaviour
{
    [SerializeField] private float damage = 50f;
    [SerializeField] private float bulletLifetime = 30f;
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private float speed = 20f;

    private Coroutine lifeTimeCoroutine;
    private bool isHeld = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        lifeTimeCoroutine = StartCoroutine(BulletLifeTime());
    }

    private IEnumerator BulletLifeTime()
    {
        float timer = 0f;

        while (timer < bulletLifetime)
        {
            if (isHeld)
            {
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHeld) return;

        int collidedLayer = collision.gameObject.layer;

        if (collidedLayer == LayerMask.NameToLayer("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collidedLayer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else
        {
            BounceOff(collision.contacts[0].normal);
        }
    }

    private void BounceOff(Vector2 collisionNormal)
    {
        Vector2 bounceDirection = Vector2.Reflect(rb.velocity.normalized, collisionNormal);
        rb.velocity = bounceDirection * bounceForce;
    }

    public void CancelLifeTimeCoroutine()
    {
        isHeld = true;
        if (lifeTimeCoroutine != null)
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null;
        }
    }
}
