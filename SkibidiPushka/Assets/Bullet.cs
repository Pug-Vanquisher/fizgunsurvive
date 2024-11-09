using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 50f; // Урон
    [SerializeField] private float bulletLifetime = 5f;  // Время жизни пули

    private void Start()
    {
        // Уничтожаем пулю через определённое время, если она не попала
        Destroy(gameObject, bulletLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Определяем слой объекта, в который попала пуля
        int collidedLayer = collision.gameObject.layer;

        // Если пуля попала в объект на слое "Player"
        if (collidedLayer == LayerMask.NameToLayer("Player"))
        {
            // Наносим урон игроку через компонент PlayerHealth
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        // Если пуля попала в объект на слое "Wall" или "PlayerTouched"
        else if (collidedLayer == LayerMask.NameToLayer("Wall") || collidedLayer == LayerMask.NameToLayer("PlayerTouched"))
        {
            // Наносим урон через компонент ObjectController
            ObjectController objectController = collision.gameObject.GetComponent<ObjectController>();
            if (objectController != null)
            {
                objectController.TakeDamage(damage);
            }
        }

        // Наносим урон самой пуле, чтобы она уничтожилась при попадании
        TakeDamage(damage);
    }

    // Наносит урон самой пуле для её уничтожения при столкновении
    private void TakeDamage(float damage)
    {
        // Уничтожаем пулю при столкновении
        Destroy(gameObject);
    }
}
