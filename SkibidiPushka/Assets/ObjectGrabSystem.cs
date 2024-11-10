using UnityEngine;

public class ObjectGrabSystem : MonoBehaviour
{
    [SerializeField] private LayerMask grabbableLayer;
    private MonoBehaviour currentObject; // Изменено на MonoBehaviour для универсальности

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

            if (objectController != null && objectController.canBeGrabbed)
            {
                currentObject = objectController;
                objectController.GrabObject();
            }
            else if (bulletController != null && bulletController.canBeGrabbed)
            {
                // Получаем компонент Bullet, чтобы отменить его время жизни
                Bullet bulletScript = hit.collider.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.CancelLifeTimeCoroutine(); // Останавливаем корутину уничтожения
                }

                // Удаляем компонент Bullet, потому что пуля теперь будет обычным объектом
                Destroy(bulletScript);

                currentObject = bulletController;
                bulletController.GrabObject();
            }
            else
            {
                currentObject = null;
            }
        }
    }
}
