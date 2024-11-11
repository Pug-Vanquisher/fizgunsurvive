using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectController : MonoBehaviour
{
    public string playerTouchedLayer = "PlayerTouched";
    public bool canBeGrabbed = false;

    [Header("Настройки объекта")]
    [SerializeField] private float massForDamageCalculation = 5f; 
    [SerializeField] private float maxhealth = 100f;
    [SerializeField] private float minDamage = 5f;
    [SerializeField] private float maxDamage = 50f;
    [SerializeField] private float breakThreshold = 10f;
    [SerializeField] private float knockbackMultiplier = 0.5f;

    private float health;
    private Rigidbody2D rb;
    private Collider2D objectCollider;
    private Renderer objectRenderer;
    private bool isHeld = false;
    private Vector2 lastPosition;
    private float currentSpeed = 0f;

    private float destroyDelay = 0.1f;


    private float maxForce;
    private float maxPulses;
    private float maxScale;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();
        objectRenderer = GetComponent<Renderer>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        health = maxhealth;

        NullizeShader();

    }

    public void GrabObject()
    {
        if (canBeGrabbed)
        {
            isHeld = true;
            gameObject.layer = LayerMask.NameToLayer(playerTouchedLayer);

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            EventManager.Instance.TriggerEvent("StartAttack");
        }
    }

    public void ReleaseObject()
    {
        if (isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;

            rb.velocity = (transform.position - (Vector3)lastPosition) / Time.deltaTime;

            EventManager.Instance.TriggerEvent("StopAttack");
        }
    }

    private void Update()
    {
        if (isHeld)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;

            currentSpeed = ((Vector2)transform.position - lastPosition).magnitude / Time.deltaTime;
            lastPosition = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHeld && collision.collider.CompareTag("Enemy"))
        {
            float damage = CalculateDamage();
            collision.collider.GetComponent<EnemyHealth>()?.TakeDamage(damage);

            ApplyKnockback(collision.collider, damage);
            TakeDamage(damage);
        }
    }

    private float CalculateDamage()
    {
        float damage = massForDamageCalculation * currentSpeed * 0.1f;
        return Mathf.Clamp(damage, minDamage, maxDamage);
    }

    private void ApplyKnockback(Collider2D enemy, float damage)
    {
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            float knockbackForce = damage * knockbackMultiplier;
            enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Объект получил урон: {damage}. Текущее здоровье: {health}");

        if (health <= 0)
        {
            DestroyObject();
        }

        ShaderLerp();
    }

    private void DestroyObject()
    {
        Debug.Log("Объект разрушен!");
        Invoke(nameof(DestroyAfterDelay), destroyDelay);
    }

    private void DestroyAfterDelay()
    {
        Destroy(gameObject);
    }

    void NullizeShader()
    {
        maxForce = GetComponent<SpriteRenderer>().material.GetFloat("_Force");
        maxPulses = GetComponent<SpriteRenderer>().material.GetFloat("_Pulses");
        maxScale = GetComponent<SpriteRenderer>().material.GetFloat("_Scale");
        GetComponent<SpriteRenderer>().material.SetFloat("_Force", 0);
        GetComponent<SpriteRenderer>().material.SetFloat("_Pulses", 0);
        GetComponent<SpriteRenderer>().material.SetFloat("_Scale", 0);
    }

    void ShaderLerp()
    {
        GetComponent<SpriteRenderer>().material.SetFloat("_Force", health * maxForce / maxhealth);
        GetComponent<SpriteRenderer>().material.SetFloat("_Pulses", health * maxPulses / maxhealth);
        GetComponent<SpriteRenderer>().material.SetFloat("_Scale", health * maxScale / maxhealth);
    }

}
