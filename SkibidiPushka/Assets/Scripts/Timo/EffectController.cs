using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] BaseEffect[] effects;
    [SerializeField] float minTime_delay;
    [SerializeField] float maxTime_delay;
    private BaseEffect _chosenOne; // by player
    private BaseEffect[] _chosenThree; // by script
    private float _currTime = 0;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (_currTime <= 0)
        {
            _currTime = Random.Range(minTime_delay, maxTime_delay);
            ChooseEffect();
        }
        else
        {
            _currTime -= Time.deltaTime;
        }
    }

    private void ChooseEffect()
    {
        ChooseThree();
    }

    private void ChooseThree()
    {

    }
}
