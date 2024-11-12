using System.Collections;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    bool waiting;

    private void Update()
    {
        Camera.main.backgroundColor = DatabaseEffects.LastUpgradeColor;
    }
}
