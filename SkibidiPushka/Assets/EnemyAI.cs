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
    [SerializeField] private float pathUpdateInterval = 0.2f; // Интервал обновления пути

    [Header("Настройки Пушек")]
    [SerializeField] private GameObject[] GunsList;
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;

    private bool isStoppedDueToDamage = false;
    private bool canShoot = true;
    private bool _isFlipped = false;
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
        GenerateGun(Random.Range(0, GunsList.Length));

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
            Vector2 randomOffset = Random.insideUnitCircle * 0.006f;
            Vector3 targetPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            agent.SetDestination(targetPosition);
        }
    }

    private void UpdateWalkingAnimation()
    {
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
        animator.SetFloat("X", agent.velocity.normalized.x);
        animator.SetFloat("Speed", speed);
    }

    private void StopMovementOnDamage()
    {
        isStoppedDueToDamage = true;
        agent.enabled = false;
        rb.velocity = Vector2.zero;
        animator.SetFloat("Hitted", 1f);
    }

    private void ResumeMovement()
    {
        isStoppedDueToDamage = false;
        agent.enabled = true;
        agent.isStopped = false;
        animator.SetFloat("Hitted", 0f);
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDamageTaken -= StopMovementOnDamage;
            enemyHealth.OnInvulnerabilityEnd -= ResumeMovement;
        }
    }

    void GenerateGun(int id)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        var NewGun = Instantiate(GunsList[id], transform);
        NewGun.GetComponent<GunScript>().detectionRange = detectionRange;
        NewGun.GetComponent<GunScript>().playerLayer = playerLayer;
        NewGun.GetComponent<GunScript>().obstacleLayer = obstacleLayer;
    }

    
}
