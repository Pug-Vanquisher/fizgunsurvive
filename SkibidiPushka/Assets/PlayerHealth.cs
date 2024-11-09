using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("��������� �������� ������")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        // �������������� �������� ������
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        // ��������� �������� ������
        currentHealth -= damage;
        Debug.Log($"����� ������� ����: {damage}. ������� ��������: {currentHealth}");

        // ���������, ���� �������� ������ ��� ����� ����
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // ������ ������ ������, ��������, ������������ ������
        Debug.Log("����� �����!");
        // ����� ����� �������� ������ ��� ����������� ������
    }
}
