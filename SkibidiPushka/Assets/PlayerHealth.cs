using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("��������� �������� ������")]
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"����� ������� ����: {damage}. ������� ��������: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        Invoke("StopHit", 1f);

    }
    
    void StopHit()
    {

    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log($"����� ����������� ��������: {amount}. ������� ��������: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("����� �����");
    }
}
