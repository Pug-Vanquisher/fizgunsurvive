using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("��������� �������� ������")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] float animDuration;
    [SerializeField] Animator animator;
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
        Animation("Hitted");
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
        Animation("IsDead");
        Debug.Log("����� �����");
    }

    private IEnumerator Animation(string name)
    {
        animator.SetFloat(name, 1);
        yield return new WaitForSeconds(animDuration);
        animator.SetFloat(name, 1);
    }


}
