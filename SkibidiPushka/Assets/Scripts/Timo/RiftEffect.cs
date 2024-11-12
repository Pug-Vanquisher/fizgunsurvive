using UnityEngine;

public class RiftEffect : MonoBehaviour
{
    public string[] Names;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

        }
    }
}
