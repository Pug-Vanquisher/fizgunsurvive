using UnityEngine;

public class RiftSpawner : MonoBehaviour
{
    [Header("Настройки разлома")]
    public float checkInterval = 15f;
    public float checkDistance = 5f;
    public LayerMask playerLayer;
    public string playerTag = "Player";

    private Transform playerTransform;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            CheckDistanceToPlayer();
            timer = 0f;
        }
    }

    private void CheckDistanceToPlayer()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        if (playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance <= checkDistance)
            {
                TryCreateRift();
            }
        }
    }

    private void TryCreateRift()
    {
        if (RiftManager.Instance.CanAddRift() && RiftManager.Instance.ShouldCreateRift())
        {
            GameObject rift = Instantiate(RiftManager.Instance.riftPrefab, transform.position, Quaternion.identity);
            RiftManager.Instance.AddRift(rift);
            Destroy(gameObject);
        }
    }
}

