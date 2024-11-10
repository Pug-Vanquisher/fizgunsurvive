using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 50f; // Урон
    [SerializeField] private float bulletLifetime = 5f;  // Время жизни пули

    private Coroutine lifeTimeCoroutine;
    private bool isHeld = false; // Флаг для проверки, что пуля захвачена

    private void Start()
    {
        // Запускаем корутину для отслеживания времени жизни пули
        lifeTimeCoroutine = StartCoroutine(BulletLifeTime());
    }

    private IEnumerator BulletLifeTime()
    {
        float timer = 0f;

        while (timer < bulletLifetime)
        {
            // Проверяем, захвачена ли пуля
            if (isHeld)
            {
                yield break; // Прерываем выполнение корутины, если пуля захвачена
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Уничтожаем пулю, если она не захвачена
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Если пуля захвачена, не обрабатываем столкновение
        if (isHeld) return;

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

    // Метод для отмены времени жизни пули при захвате
    public void CancelLifeTimeCoroutine()
    {
        isHeld = true;
        if (lifeTimeCoroutine != null)
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null; // Очищаем ссылку на корутину
        }
    }
}
