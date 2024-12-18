using UnityEngine;
using System.Collections.Generic;


public class EffectController : MonoBehaviour
{

    public float effectDuration;

    public GameObject ConsolePrefab;

    private float timer;
    public List<BaseEffect> effects = new List<BaseEffect>();
    public RiftEffect rEffect;
    public bool isChoosing = true;

    private void Awake()
    {
        EventManager.Instance.Subscribe("UpgradeTime", AwakeUpgradeConsole);
        EventManager.Instance.Subscribe("RiftClosed", RiftEffectSpawn);
    }
    void AwakeUpgradeConsole()
    {
        var a = Instantiate(ConsolePrefab);
        for (int i = 0; i < 3; i++)
        {
            effects.Add(DatabaseEffects.effects[Random.Range(0, DatabaseEffects.effects.Count)]);
        }
        a.GetComponent<ConsoleScript>().effects = effects.ToArray();
        EventManager.Instance.TriggerEvent("Upscaling");
        StartCoroutine(RltInvoke());
    }
    void RiftEffectSpawn()
    {
        rEffect = DatabaseRiftEffects.effects[Random.Range(0, DatabaseRiftEffects.effects.Count)];

        EventManager.Instance.TriggerEvent(rEffect.Event);
    }
    IEnumerator<WaitForSecondsRealtime> RltInvoke()
    {
        yield return new WaitForSecondsRealtime(3f);
        isChoosing = true;
    }

    private void Update()
    {
        if (isChoosing)
        {
            ChooseEffect();
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (timer >= effectDuration)
        {
            timer = 0;
            EventManager.Instance.TriggerEvent("UpgradeTime");
        }

    }

    private void ChooseEffect()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            MakeEvent(0);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            MakeEvent(1);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            MakeEvent(2);
        }
    }
    private void MakeEvent(int id)
    {
        EventManager.Instance.TriggerEvent(effects[id].Event);
        DatabaseEffects.LastUpgradeColor = effects[id].Color;
        effects.RemoveAt(id);
        EventManager.Instance.TriggerEvent(effects[Random.Range(0, effects.Count)].Event);
        effects.Clear();
        isChoosing = false; 
    }
}
