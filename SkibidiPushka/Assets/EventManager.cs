using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("EventManager");
                _instance = obj.AddComponent<EventManager>();
            }
            return _instance;
        }
    }

    // ������� �������
    private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();

    // ����� ��� ��������
    public void Subscribe(string eventName, UnityAction listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = new UnityEvent();
        }
        eventDictionary[eventName].AddListener(listener);
    }

    // ����� ��� �������
    public void Unsubscribe(string eventName, UnityAction listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName].RemoveListener(listener);
        }
    }

    // ����� ��� ������ �������
    public void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName].Invoke();
        }
    }
}
