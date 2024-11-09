using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("��������� ��������")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invulnerabilityTime = 0.5f;

    public delegate void DamageAction();
    public event DamageAction OnDamageTaken;
    public event DamageAction OnInvulnerabilityEnd;

    private float currentHealth;
    private bool isInvulnerable = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        Debug.Log($"���� ������� ����: {damage}. ������� ��������: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnDamageTaken?.Invoke();
            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    private void Die()
    {
        Debug.Log("���� ���������");
        Destroy(gameObject);
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
        OnInvulnerabilityEnd?.Invoke();
    }
}
