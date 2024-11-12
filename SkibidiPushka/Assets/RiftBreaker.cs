using UnityEngine;
using System.Collections;
public class RiftBreaker : MonoBehaviour
{
    private bool closed = false;
    public float ClosingTime = 1f;

    private Material PortalShader;
    public SpriteRenderer Portal;

    private void Start()
    {
        PortalShader = Portal.material;
        StartCoroutine(Opening());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            RiftCorrupterPlacer activity = GetComponent<RiftCorrupterPlacer>();
            if (!closed)
            {
                if (activity != null)
                {
                    Destroy(activity);
                }

                //Trigger

                StartCoroutine(Closing());
            }
        }
    }

    IEnumerator Closing()
    {
        float timer = 0f;
        PortalShader.SetFloat("_Brightness", -2);

        while (timer < ClosingTime)
        {
            timer += Time.deltaTime;
            PortalShader.SetFloat("_Brightness", Mathf.Lerp(PortalShader.GetFloat("_Brightness"), 5f, 0.01f));
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
        yield return null;
    }
    IEnumerator Opening()
    {
        float timer = 0f;
        PortalShader.SetFloat("_Brightness", 5);
        while (timer < ClosingTime)
        {
            timer += Time.deltaTime;
            PortalShader.SetFloat("_Brightness", Mathf.Lerp(PortalShader.GetFloat("_Brightness"), -0.2f, 0.1f));
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}

