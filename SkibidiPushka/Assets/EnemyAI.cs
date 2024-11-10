using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Настройки врага")]
    public Transform player;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 1.5f;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float sphereCastRadius = 0.5f;
    [SerializeField] private float sphereCastDistance = 0.5f;
    [SerializeField] private float pathUpdateInterval = 0.2f; // Интервал обновления пути

    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;

    private bool isStoppedDueToDamage = false;
    private bool canShoot = true;
    private Vector3 lastPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (enemyHealth != null)
        {
            enemyHealth.OnDamageTaken += StopMovementOnDamage;
            enemyHealth.OnInvulnerabilityEnd += ResumeMovement;
        }
        StartCoroutine(UpdatePathPeriodically());
    }

    private void Update()
    {
        if (player == null || isStoppedDueToDamage) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Проверка на видимость игрока и стрельба
        if (distanceToPlayer <= detectionRange && IsPlayerInSight())
        {
            TryShootAtPlayer();
        }

        FlipSprite();
        CheckForObstacles();
        UpdateWalkingAnimation();
    }

    private IEnumerator UpdatePathPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(pathUpdateInterval);

            if (player != null && !isStoppedDueToDamage)
            {
                MoveToRandomPointNearPlayer();
            }
        }
    }

    private void MoveToRandomPointNearPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= stopDistance)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            Vector2 randomOffset = Random.insideUnitCircle * 6f;
            Vector3 targetPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            agent.SetDestination(targetPosition);
        }
    }

    private bool IsPlayerInSight()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, playerLayer);
        return hit.collider != null && hit.collider.CompareTag("Player");
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

    private void ShootBullet(Vector3 targetPosition)
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            Vector2 direction = (targetPosition - firePoint.position).normalized;
            bulletRb.velocity = direction * bulletSpeed;
        }
    }

    private void ResetShoot()
    {
        canShoot = true;
    }

    private void CheckForObstacles()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, sphereCastRadius, transform.right, sphereCastDistance, obstacleLayer);

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

    private void UpdateWalkingAnimation()
    {
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        animator.SetBool("Walk", speed > 0.05f);
    }

    private void FlipSprite()
    {
        if (agent.velocity.x > 0.1f)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (agent.velocity.x < -0.1f)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void StopMovementOnDamage()
    {
        isStoppedDueToDamage = true;
        agent.enabled = false;
        rb.velocity = Vector2.zero;
        animator.SetBool("Walk", false);
        animator.SetBool("Hit", true);
    }

    private void ResumeMovement()
    {
        isStoppedDueToDamage = false;
        agent.enabled = true;
        agent.isStopped = false;
        animator.SetBool("Hit", false);
        animator.SetBool("Walk", true);
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDamageTaken -= StopMovementOnDamage;
            enemyHealth.OnInvulnerabilityEnd -= ResumeMovement;
        }
    }
}
