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
        FreezeFrame(0.1f);
    }

    public void FreezeFrame(float dur)
    {
        if (waiting) return;
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
