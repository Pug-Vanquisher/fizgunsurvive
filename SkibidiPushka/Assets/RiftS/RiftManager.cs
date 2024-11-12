using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using NUnit.Framework.Internal;

public class RiftManager : MonoBehaviour
{
    public static RiftManager Instance;

    [Header("Настройки разломов")]
    public int maxRifts = 3;
    [Range(0, 1)] public float riftProbability = 0.1f; 
    public GameObject riftPrefab;

    private int currentRiftCount = 0;
    private List<GameObject> rifts = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            EventManager.Instance.Subscribe("RiftStabilize", ResetRifts);
            EventManager.Instance.Subscribe("HighVolatility", IncreaseRiftParameters);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            EventManager.Instance.Unsubscribe("RiftStabilize", ResetRifts);
            EventManager.Instance.Unsubscribe("HighVolatility", IncreaseRiftParameters);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetRiftParameters();
    }

    public void AddRift(GameObject rift)
    {
        if (currentRiftCount < maxRifts)
        {
            rifts.Add(rift);
            currentRiftCount++;
        }
    }

    public void RemoveRift(GameObject rift)
    {
        if (rifts.Contains(rift))
        {
            rifts.Remove(rift);
            currentRiftCount--;
        }
    }

    public bool CanAddRift()
    {
        return currentRiftCount < maxRifts;
    }

    public bool ShouldCreateRift()
    {
        return Random.value <= riftProbability;
    }

    public void ResetRifts()
    {
        foreach (GameObject rift in rifts)
        {
            Destroy(rift);
        }
        rifts.Clear();
        currentRiftCount = 0;
    }

    public void IncreaseRiftParameters()
    {
        maxRifts++;
        riftProbability = Mathf.Clamp01(riftProbability + 0.05f); 
    }

    public void ResetRiftParameters()
    {
        maxRifts = 3;
        riftProbability = 0.1f;
    }


}
