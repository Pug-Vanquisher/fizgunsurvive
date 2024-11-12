using System.Collections;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    bool waiting;

    private void Start()
    {
        EventManager.Instance.Subscribe("Explosion", ExplosionFreeze); 
    }

    public void ExplosionFreeze() 
    {
        FreezeFrame(0.05f);
    }
    private void Update()
    {
        Camera.main.backgroundColor = DatabaseEffects.LastUpgradeColor;
    }
    public void FreezeFrame(float dur)
    {
        if (waiting || Time.timeScale != 1) return;
        Time.timeScale = 0.001f;
        StartCoroutine(Wait(dur));
    }

    IEnumerator Wait (float dur)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime (dur);
        Time.timeScale = 1;
        waiting = false;
    }
}
