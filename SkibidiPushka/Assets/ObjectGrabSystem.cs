using UnityEngine;

public class ObjectGrabSystem : MonoBehaviour
{
    [SerializeField] private LayerMask grabbableLayer;
    private ObjectController currentObject;

    private void Update()
    {
        HandleObjectGrab();
    }

    private void HandleObjectGrab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject();
        }
        else if (Input.GetMouseButtonUp(0) && currentObject != null)
        {
            currentObject.ReleaseObject();
            currentObject = null;
        }
    }

    private void TryGrabObject()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0.1f, grabbableLayer);

        if (hit.collider != null)
        {
            currentObject = hit.collider.GetComponent<ObjectController>();
            if (currentObject != null && currentObject.canBeGrabbed) // Проверка флага захвата
            {
                currentObject.GrabObject();
            }
            else
            {
                currentObject = null;
            }
        }
    }
}
