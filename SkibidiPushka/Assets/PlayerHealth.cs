using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Настройки здоровья игрока")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] Menu menu;
    public float currentHealth;

    [SerializeField] AudioClip clip;

    private float maxForce;
    private float maxPulses;
    private float maxScale;

    private void Start()
    {
        currentHealth = maxHealth;

        EventManager.Instance.Subscribe("HighHealth", HealthUpscale);
    }
    
    public void HealthUpscale()
    {
        float newHeal = currentHealth * (maxHealth + 100) / maxHealth;
        maxHealth += 100;
        Heal(newHeal);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Игрок получил урон: {damage}. Текущее здоровье: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        GetComponent<Animator>().SetFloat("Hitted", 1f);
        Invoke("StopHit", 0.4f);

    }
    
    void StopHit()
    {
        GetComponent<Animator>().SetFloat("Hitted", 0f);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        SFXManager.instance.PlaySound(clip, transform);
        Debug.Log($"Игрок восстановил здоровье: {amount}. Текущее здоровье: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("Игрок погиб");
        //Destroy(gameObject);
        menu.gameObject.SetActive(true);
        menu.EndGame();
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
