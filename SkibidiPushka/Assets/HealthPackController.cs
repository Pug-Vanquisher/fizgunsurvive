using UnityEngine;

public class HealthPackController : MonoBehaviour
{
    [SerializeField] private float healthRestoreAmount = 50f;
    private SpecialObjectController specialObjectController;

    private void Awake()
    {
        specialObjectController = GetComponent<SpecialObjectController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (specialObjectController != null)
            {
                specialObjectController.OnUse();
            }

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healthRestoreAmount);

                Destroy(gameObject); 
            }
        }
    }
}
