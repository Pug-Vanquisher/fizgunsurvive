using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] private float timeToDestruct = 5f; 

    private void Start()
    {
        SFXManager.instance.PlaySound(clip, transform);
        StartCoroutine(DestructAfterTime());
    }

    private IEnumerator DestructAfterTime()
    {
        yield return new WaitForSeconds(timeToDestruct);

        Destroy(gameObject);
    }
}
