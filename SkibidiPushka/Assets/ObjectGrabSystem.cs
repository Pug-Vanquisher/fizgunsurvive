using UnityEngine;

public class ObjectGrabSystem : MonoBehaviour
{
    [SerializeField] private LayerMask grabbableLayer;
    private MonoBehaviour currentObject;

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
            if (currentObject is ObjectController objectController)
            {
                objectController.ReleaseObject();
            }
            else if (currentObject is BulletController bulletController)
            {
                bulletController.ReleaseObject();
            }
            else if (currentObject is GranadeController granadeController)
            {
                granadeController.ReleaseObject();
            }

            currentObject = null;
        }
    }

    private void TryGrabObject()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0.1f, grabbableLayer);

        if (hit.collider != null)
        {
            ObjectController objectController = hit.collider.GetComponent<ObjectController>();
            BulletController bulletController = hit.collider.GetComponent<BulletController>();
            GranadeController granadeController = hit.collider.GetComponent<GranadeController>();

            if (objectController != null && objectController.canBeGrabbed)
            {
                currentObject = objectController;
                objectController.GrabObject();
            }
            else if (bulletController != null && bulletController.canBeGrabbed)
            {
                Bullet bulletScript = hit.collider.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.CancelLifeTimeCoroutine();
                }

                Destroy(bulletScript);

                currentObject = bulletController;
                bulletController.GrabObject();
            }
            else if (granadeController != null && granadeController.canBeGrabbed)
            {
                currentObject = granadeController;
                granadeController.GrabObject();
            }
            else
            {
                currentObject = null;
            }
        }
    }
}
