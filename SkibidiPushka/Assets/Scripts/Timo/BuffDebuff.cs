using UnityEngine;

public class BuffDebuff : MonoBehaviour // потом засунуть в наследование каждого уникального эффекта
{
    internal string Name;

    private void Awake()
    {
        EventManager.Instance.Subscribe(Name, StartEffect);
        EventManager.Instance.Subscribe(Name + "Stop", StopEffect);
    }

    protected virtual void StartEffect()
    {

    }

    protected virtual void StopEffect()
    {

    }

    protected virtual void Effect()
    {

    }
    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(Name, StartEffect);
        EventManager.Instance.Unsubscribe(Name + "Stop", StopEffect);
    }


}
