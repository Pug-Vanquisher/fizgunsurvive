using UnityEngine;

public class RocketLauncher : GunScript
{
    protected override void ShootBullet(Vector3 targetPosition)
    {
        base.ShootBullet(targetPosition);
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 direction = (targetPosition - firePoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject rocket = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        Rigidbody2D rocketRb = rocket.GetComponent<Rigidbody2D>();

        if (rocketRb != null)
        {
            rocketRb.velocity = direction * bulletSpeed;
        }
    }
}
