using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Start()
    {
        RiftManager.Instance.ResetRifts();
        RiftManager.Instance.ResetRiftParameters();
    }
}
