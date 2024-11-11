using UnityEngine;
using System.Collections;
public class PistolScript : GunScript
{
    protected override void ShootBullet(Vector3 targetPosition)
    {
        base.ShootBullet(targetPosition);
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            Vector2 direction = (targetPosition - firePoint.position).normalized;
            bulletRb.velocity = direction * bulletSpeed;
        }
    }
}
