using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Настройки здоровья игрока")]
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Игрок получил урон: {damage}. Текущее здоровье: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб");
    }
}
