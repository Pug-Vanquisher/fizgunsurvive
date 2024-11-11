using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MineController : MonoBehaviour
{
    public string touchedMineLayer = "TouchedMine";
    public bool canBeGrabbed = false;
    public float moveForce = 10f;

    private Rigidbody2D rb;
    private bool isHeld = false;

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
            Vector2 direction = (mousePos - rb.position).normalized;
            rb.AddForce(direction * moveForce);
        }
    }

    public void GrabObject()
    {
        if (canBeGrabbed)
        {
            isHeld = true;
            gameObject.layer = LayerMask.NameToLayer(touchedMineLayer);

            EventManager.Instance.TriggerEvent("StartAttack");
        }
    }

    public void Detonated()
    {
        if (isHeld)
        {
            EventManager.Instance.TriggerEvent("StopAttack");
        }
    }

    public void ReleaseObject()
    {
        if (isHeld)
        {
            isHeld = false;
            //rb.velocity = Vector2.zero;

            EventManager.Instance.TriggerEvent("StopAttack");
        }
    }
}
