using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Настройки здоровья игрока")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        // Инициализируем здоровье игрока
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        // Уменьшаем здоровье игрока
        currentHealth -= damage;
        Debug.Log($"Игрок получил урон: {damage}. Текущее здоровье: {currentHealth}");

        // Проверяем, если здоровье меньше или равно нулю
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Логика смерти игрока, например, перезагрузка уровня
        Debug.Log("Игрок погиб!");
        // Здесь можно добавить логику для перезапуска уровня
    }
}
