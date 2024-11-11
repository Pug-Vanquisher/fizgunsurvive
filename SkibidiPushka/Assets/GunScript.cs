using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Header("Из врага")]
    protected Transform player;
    [SerializeField] public float detectionRange = 10f;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public LayerMask obstacleLayer;

    [Header("Настройка пушки")]
    public Transform firePoint;
    [SerializeField] private float sphereCastRadius = 0.5f;
    [SerializeField] private float sphereCastDistance = 0.5f;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float fireCooldown;
    [SerializeField] public bool canShoot;


    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.parent.position, player.position);

        // Проверка на видимость игрока и стрельба
        if (distanceToPlayer <= detectionRange && IsPlayerInSight())
        {
            TryShootAtPlayer();
        }
        CheckForObstacles();
    }
    private bool IsPlayerInSight()
    {
        Vector2 directionToPlayer = (player.position - transform.parent.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.parent.position, directionToPlayer, detectionRange, playerLayer);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    private void CheckForObstacles()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.parent.position, sphereCastRadius, transform.right, sphereCastDistance, obstacleLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall") ||
                hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerTouched"))
            {
                if (canShoot)
                {
                    canShoot = false;
                    ShootBullet(hit.point);
                    Invoke(nameof(ResetShoot), fireCooldown);
                }
            }
        }
    }

    private void TryShootAtPlayer()
    {
        if (canShoot)
        {
            canShoot = false;
            ShootBullet(player.position);
            Invoke(nameof(ResetShoot), fireCooldown);
        }
    }
    private void ResetShoot()
    {
        canShoot = true;
    }

    protected virtual void ShootBullet(Vector3 targetPosition)
    {

    }

}
