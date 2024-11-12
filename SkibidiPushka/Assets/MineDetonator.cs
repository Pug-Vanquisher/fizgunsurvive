using UnityEngine;
using System.Collections;

public class MineDetonator : MonoBehaviour
{
    public float explosionDelay = 1.5f; // �������� ������
    public float damage = 100f; // ����
    public float explosionRadius = 4f; // ������ ������
    public GameObject explosionEffectPrefab;

    [SerializeField] AudioClip clip;

    public Animator animator;

    private bool isExploding = false;

    private MineController mineController;

    //� ��� � ������.
    public Transform player;

    private void Awake()
    {
        mineController = GetComponent<MineController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int collidedLayer = collision.gameObject.layer;

        if (collidedLayer == LayerMask.NameToLayer("Wall") ||
            collidedLayer == LayerMask.NameToLayer("PlayerTouched") ||
            collidedLayer == LayerMask.NameToLayer("Player") ||
            collidedLayer == LayerMask.NameToLayer("Enemy"))
        {
            if (!isExploding)
            {
                StartCoroutine(ExplodeAfterDelay());
            }
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        isExploding = true;

        animator.SetBool("AMABATACUUUM", true);

        SFXManager.instance.PlaySound(clip, transform);

        yield return new WaitForSeconds(explosionDelay);

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

        //��� � ������� >:)

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= 10)
        {
            EventManager.Instance.TriggerEvent("Explosion");
        }

        //����.

        if (mineController != null)
        {
            mineController.Detonated();
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
