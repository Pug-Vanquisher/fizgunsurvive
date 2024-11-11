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
            else if (currentObject is SpecialObjectController specialObjectController)
            {
                specialObjectController.ReleaseObject();
            }
            else if (currentObject is MineController mineController)
            {
                mineController.ReleaseObject();
            }
            else if (currentObject is RocketController rocketController)
            {
                rocketController.ReleaseObject();
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
            SpecialObjectController specialObjectController = hit.collider.GetComponent<SpecialObjectController>();
            MineController mineController = hit.collider.GetComponent<MineController>();
            RocketController rocketController = hit.collider.GetComponent<RocketController>();

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
                    Destroy(bulletScript);
                }
                else
                {
                    WildBullet wildBulletScript = hit.collider.GetComponent<WildBullet>();
                    if (wildBulletScript != null)
                    {
                        wildBulletScript.CancelLifeTimeCoroutine();
                        Destroy(wildBulletScript);
                    }
                }

                currentObject = bulletController;
                bulletController.GrabObject();
            }
            else if (granadeController != null && granadeController.canBeGrabbed)
            {
                currentObject = granadeController;
                granadeController.GrabObject();
            }
            else if (specialObjectController != null && specialObjectController.canBeGrabbed)
            {
                currentObject = specialObjectController;
                specialObjectController.GrabObject();
            }
            else if (mineController != null && mineController.canBeGrabbed)
            {
                currentObject = mineController;
                mineController.GrabObject();
            }
            else if (rocketController != null && rocketController.canBeGrabbed)
            {
                Rocket rocketScript = hit.collider.GetComponent<Rocket>();
                if (rocketScript != null)
                {
                    rocketScript.CancelLifeTimeCoroutine();
                    Destroy(rocketScript);
                }
                else
                {
                    WildRocket wildRocketScript = hit.collider.GetComponent<WildRocket>();
                    if (wildRocketScript != null)
                    {
                        wildRocketScript.CancelLifeTimeCoroutine();
                        Destroy(wildRocketScript);
                    }
                }

                currentObject = rocketController;
                rocketController.GrabObject();
            }
            else
            {
                currentObject = null;
            }
        }
    }
}
