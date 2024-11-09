using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 50f; // ����
    [SerializeField] private float bulletLifetime = 5f;  // ����� ����� ����

    private void Start()
    {
        // ���������� ���� ����� ����������� �����, ���� ��� �� ������
        Destroy(gameObject, bulletLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������� ���� �������, � ������� ������ ����
        int collidedLayer = collision.gameObject.layer;

        // ���� ���� ������ � ������ �� ���� "Player"
        if (collidedLayer == LayerMask.NameToLayer("Player"))
        {
            // ������� ���� ������ ����� ��������� PlayerHealth
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        // ���� ���� ������ � ������ �� ���� "Wall" ��� "PlayerTouched"
        else if (collidedLayer == LayerMask.NameToLayer("Wall") || collidedLayer == LayerMask.NameToLayer("PlayerTouched"))
        {
            // ������� ���� ����� ��������� ObjectController
            ObjectController objectController = collision.gameObject.GetComponent<ObjectController>();
            if (objectController != null)
            {
                objectController.TakeDamage(damage);
            }
        }

        // ������� ���� ����� ����, ����� ��� ������������ ��� ���������
        TakeDamage(damage);
    }

    // ������� ���� ����� ���� ��� � ����������� ��� ������������
    private void TakeDamage(float damage)
    {
        // ���������� ���� ��� ������������
        Destroy(gameObject);
    }
}
