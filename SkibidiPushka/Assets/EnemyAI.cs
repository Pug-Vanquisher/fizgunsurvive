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
    [SerializeField] private float pathUpdateInterval = 0.2f;

    [Header("Настройки Пушек")]
    [SerializeField] private GameObject[] GunsList;

    [Header("Настройки префаба для замены")]
    [SerializeField] private GameObject replacementPrefab;

    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;
    private EnemyController enemyController;

    private bool isStoppedDueToDamage = false;
    private Vector3 lastPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyController = GetComponent<EnemyController>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        GenerateGun(Random.Range(0, GunsList.Length));

        EventManager.Instance.Subscribe("EnemiesToWalls", ReplaceWithPrefab);

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null && playerObj.scene.IsValid())
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player not found");
            }
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
        if (player == null || isStoppedDueToDamage || (enemyController != null && enemyController.IsHeld)) return;
        Debug.Log($"Enemy Position: {transform.position}, Player Position: {player.position}");

        UpdateWalkingAnimation();
    }

    private IEnumerator UpdatePathPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(pathUpdateInterval);

            if (player != null && !isStoppedDueToDamage && (enemyController == null || !enemyController.IsHeld))
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

            Vector2 randomOffset = Random.insideUnitCircle * 1f;
            Vector3 targetPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 2.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                agent.SetDestination(player.position);
            }
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
        SetAgentEnabled(false);
        rb.velocity = Vector2.zero;
        animator.SetFloat("Hitted", 1f);
    }

    private void ResumeMovement()
    {
        isStoppedDueToDamage = false;
        SetAgentEnabled(true);
        animator.SetFloat("Hitted", 0f);
    }

    public void SetAgentEnabled(bool enabled)
    {
        if (agent != null && (enemyController == null || !enemyController.IsHeld))
        {
            agent.enabled = enabled;
            agent.isStopped = !enabled;
        }
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
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        var NewGun = Instantiate(GunsList[id], transform);
        NewGun.GetComponent<GunScript>().detectionRange = detectionRange;
        NewGun.GetComponent<GunScript>().playerLayer = playerLayer;
        NewGun.GetComponent<GunScript>().obstacleLayer = obstacleLayer;
    }

    public void ReplaceWithPrefab()
    {
        if (replacementPrefab != null)
        {
            Instantiate(replacementPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Replacement prefab is not assigned.");
        }
    }
}
