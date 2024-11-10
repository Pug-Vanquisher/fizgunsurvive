using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.Events;

public class NavMeshUpdater : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    //private const string updateEventName = "UpdateNavMesh";

    private void Start()
    {
        if (navMeshSurface == null)
        {
            navMeshSurface = FindObjectOfType<NavMeshSurface>();
        }

        BuildNavMesh();

        //EventManager.Instance.Subscribe(updateEventName, BuildNavMesh);
        //EventManager.Instance.Subscribe("ClearedObj", BuildNavMeshDelay);
    }

    private void BuildNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh успешно обновлен!");
        }
        else
        {
            Debug.LogWarning("NavMeshSurface не найден!");
        }
    }

    //private void BuildNavMeshDelay()
    //{
    //    Invoke(nameof(BuildNavMesh), 1f);
    //}

    //private void OnDestroy()
    //{
    //    if (EventManager.Instance != null)
    //    {
    //        EventManager.Instance.Unsubscribe(updateEventName, BuildNavMesh);
    //    }
    //}
}
