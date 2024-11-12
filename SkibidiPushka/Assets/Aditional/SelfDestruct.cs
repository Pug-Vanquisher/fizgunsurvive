using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float timeToDestruct = 5f; 

    private void Start()
    {
        StartCoroutine(DestructAfterTime());
    }

    private IEnumerator DestructAfterTime()
    {
        yield return new WaitForSeconds(timeToDestruct);

        Destroy(gameObject);
    }
}
