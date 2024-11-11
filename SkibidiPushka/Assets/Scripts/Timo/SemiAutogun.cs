using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class SemiAutogun : GunScript
{
    [SerializeField] float numberOfShoots;
    [SerializeField, Range(0,30), Tooltip("в градусах")] float bulletSpread;
    [SerializeField, Range(0.1f, 0.3f)] float delayBetweenShots;
    private Quaternion rotation;
    protected override void ShootBullet(Vector3 targetPosition)
    {
        if (bulletPrefab == null || firePoint == null) return;

        StartCoroutine(SHOOTA(targetPosition));
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 forward = player.position - transform.position;
        Vector2 up = Quaternion.Euler(0, 0, bulletSpread) * forward;
        Vector2 down = Quaternion.Euler(0, 0, -bulletSpread) * forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)up * 4);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)down* 4);
    }

    IEnumerator SHOOTA(Vector3 targetPosition)
    {
        for (int shootsNum = 0; shootsNum < numberOfShoots; shootsNum++)
        {
            Vector2 spreadDirect = Vector2.zero;
            var angle = Random.Range(-bulletSpread * Mathf.Deg2Rad, bulletSpread * Mathf.Deg2Rad);
            spreadDirect.x += Mathf.Cos(angle);
            spreadDirect.y += Mathf.Sin(angle);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            Vector2 direction = (Vector2)(targetPosition - transform.position + (Vector3)spreadDirect).normalized;

            if (bulletRb != null)
            {
                bulletRb.velocity = direction * bulletSpeed;
            }
            yield return new WaitForSeconds(delayBetweenShots);
        }
    }
}
