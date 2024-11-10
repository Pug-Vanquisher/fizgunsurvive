using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 50f; 
    [SerializeField] private float bulletLifetime = 5f;  

    private Coroutine lifeTimeCoroutine;
    private bool isHeld = false; 

    private void Start()
    {
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
        }
        else if (collidedLayer == LayerMask.NameToLayer("Wall") || collidedLayer == LayerMask.NameToLayer("PlayerTouched"))
        {
            ObjectController objectController = collision.gameObject.GetComponent<ObjectController>();
            if (objectController != null)
            {
                objectController.TakeDamage(damage);
            }
        }

        TakeDamage(damage);
    }

    private void TakeDamage(float damage)
    {
        Destroy(gameObject);
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
