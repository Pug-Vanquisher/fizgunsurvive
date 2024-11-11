using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Настройки здоровья")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invulnerabilityTime = 0.5f;

    public delegate void DamageAction();
    public event DamageAction OnDamageTaken;
    public event DamageAction OnInvulnerabilityEnd;

    private float currentHealth;
    private bool isInvulnerable = false;

    private float maxForce;
    private float maxPulses;
    private float maxScale;


    private void Awake()
    {
        currentHealth = maxHealth;
        NullizeShader();
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        Debug.Log($"Враг получил урон: {damage}. Текущее здоровье: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnDamageTaken?.Invoke();
            StartCoroutine(InvulnerabilityCoroutine());
        }

        ShaderLerp();
    }

    private void Die()
    {
        Debug.Log("Враг уничтожен");
        Destroy(gameObject);
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
        OnInvulnerabilityEnd?.Invoke();
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
        GetComponent<SpriteRenderer>().material.SetFloat("_Force", currentHealth * maxForce / maxHealth);
        GetComponent<SpriteRenderer>().material.SetFloat("_Pulses", currentHealth * maxPulses / maxHealth);
        GetComponent<SpriteRenderer>().material.SetFloat("_Scale", currentHealth * maxScale / maxHealth);
    }
    

}
