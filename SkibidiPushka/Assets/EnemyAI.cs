using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Настройки врага")]
    public Transform player;
    [SerializeField] private float stopDistance = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;

    private bool isStoppedDueToDamage = false;

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
    }

    private void Update()
    {
        if (player == null || isStoppedDueToDamage) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= stopDistance)
        {
            agent.isStopped = true;
            animator.SetBool("Walk", false);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("Walk", true);
        }

        // Поворачиваем спрайт в зависимости от направления движения
        FlipSprite();
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
        rb.angularVelocity = 0f;

        animator.SetBool("Walk", false);
        animator.SetBool("Hit", true);
    }

    private void ResumeMovement()
    {
        isStoppedDueToDamage = false;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

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
