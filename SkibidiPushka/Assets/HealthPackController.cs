using UnityEngine;

public class HealthPackController : MonoBehaviour
{
    [SerializeField] private float healthRestoreAmount = 50f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healthRestoreAmount);
                Destroy(gameObject); 
            }
        }
    }
}
