using UnityEngine;
using System.Collections;

public class WildBullet : MonoBehaviour
{
    [Header("Настройки пули")]
    [SerializeField] private float damage = 50f;
    [SerializeField] private float bulletLifetime = 30f;
    [SerializeField] private float speed = 20f;

    private Vector2 currentDirection;
    private bool isHeld = false;
    private Coroutine lifeTimeCoroutine;

    private void Start()
    {
        currentDirection = transform.right;
        lifeTimeCoroutine = StartCoroutine(BulletLifeTime());
    }

    private void Update()
    {
        if (isHeld) return;

        transform.Translate(currentDirection * speed * Time.deltaTime, Space.World);
    }

    private IEnumerator BulletLifeTime()
    {
        float timer = 0f;

        while (timer < bulletLifetime)
        {
            if (isHeld)
                yield break;

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
            Vector2 collisionNormal = collision.contacts[0].normal;
            BounceOff(collisionNormal);
        }
    }

    private void BounceOff(Vector2 collisionNormal)
    {
        currentDirection = Vector2.Reflect(currentDirection, collisionNormal).normalized;

        RotateBullet();
    }

    private void RotateBullet()
    {
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
