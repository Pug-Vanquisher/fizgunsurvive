using UnityEngine;


public class EffectController : MonoBehaviour
{
    [SerializeField] BaseEffect[] effects;
    [SerializeField] float minTime_delay;
    [SerializeField] float maxTime_delay;
    [SerializeField] float timeForChoosing;
    private BaseEffect _chosenOne; // by player
    private float _currTime = 0;
    private float _timeForSpawnChoose;
    private BaseEffect[] chosenEffects;

    private void Awake()
    {
        _currTime = maxTime_delay;
    }

    private void Update()
    {
        if (_currTime > _timeForSpawnChoose)
        {
            _timeForSpawnChoose = Random.Range(Mathf.Min(minTime_delay, maxTime_delay), Mathf.Max(minTime_delay, maxTime_delay));
            ChooseEffect();
        }
        else if (_currTime < timeForChoosing + 0.1f)
        {
            ChooseEffect();
        }
            _currTime += Time.deltaTime;
    }

    private void ChooseEffect()
    {
        int chosen = -4453;
        ChooseThree();
        if (Input.GetKey(KeyCode.Alpha1))
        {
            chosen = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            chosen = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            chosen = 3;
        }
        else
        {
            if (_currTime > timeForChoosing)
            {
                chosen = (int)Random.Range(1, 3);
            }

        }
        if (!(chosen > 0))
        {
            int chosenRand = 0;
            _chosenOne = chosenEffects[chosen - 1];
            switch (chosen)
            {
                case 1:
                    chosenRand = Random.value > 0.5f ? 2:3;
                    return;
                case 2:
                    chosenRand = Random.value > 0.5f ? 1 : 3;
                    return;
                case 3:
                    chosenRand = Random.value > 0.5f ? 2 : 1;
                    return;
            }
            
            if (chosenRand != 0)
            {
            EventManager.Instance.TriggerEvent(chosenEffects[chosenRand - 1].EventName);
            }
            _currTime = 0;
        }
        if (_chosenOne != null)
        {
            EventManager.Instance.TriggerEvent(_chosenOne.EventName);
        }
        for(int x = 0; x < effects.Length; x++)
        {
            if (effects[x] == null) 
            {
                
            }
        }
        

    }

    private void ChooseThree()
    {
        var numberOfEffects = new int[3];
        numberOfEffects[0] = Random.Range(0, effects.Length);
        chosenEffects[numberOfEffects[0]] = effects[numberOfEffects[0]];
        effects[numberOfEffects[0]] = null;
        numberOfEffects[1] = Random.Range(0, effects.Length);
        if (effects[numberOfEffects[1]] == null)
        {
            chosenEffects[numberOfEffects[1]] = effects[numberOfEffects[1] + 1];
            effects[numberOfEffects[1]+1] = null;
        }
        else
        {
            chosenEffects[numberOfEffects[1]] = effects[numberOfEffects[1]];
            effects[numberOfEffects[1]] = null;
        }
        numberOfEffects[2] = Random.Range(0, effects.Length);
        if (effects[numberOfEffects[2]] == null)
        {
            chosenEffects[numberOfEffects[2]] = effects[numberOfEffects[2] + 1];
            effects[numberOfEffects[2] + 1] = null;
        }
        else
        {
            chosenEffects[numberOfEffects[2]] = effects[numberOfEffects[2]];
            effects[numberOfEffects[2]] = null;
        }
    }
}
