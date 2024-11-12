using UnityEngine;
using System.Collections;
public class PistolScript : GunScript
{
    [SerializeField] AudioClip clip;

    protected override void ShootBullet(Vector3 targetPosition)
    {
        base.ShootBullet(targetPosition);
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        SFXManager.instance.PlaySound(clip, transform);

        if (bulletRb != null)
        {
            Vector2 direction = (targetPosition - firePoint.position).normalized;
            bulletRb.velocity = direction * bulletSpeed;
        }
    }
}
