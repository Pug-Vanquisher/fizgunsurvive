using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpecialObjectController : MonoBehaviour
{
    public bool canBeGrabbed = false;
    private bool isHeld = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        if (isHeld)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
    }

    public void GrabObject()
    {
        if (canBeGrabbed)
        {
            isHeld = true;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void ReleaseObject()
    {
        if (isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
        }
    }
}
