using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.Events;

public class NavMeshUpdater : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    private const string updateEventName = "UpdateNavMesh";

    private void Start()
    {
        if (navMeshSurface == null)
        {
            navMeshSurface = FindObjectOfType<NavMeshSurface>();
        }

        BuildNavMesh();

        EventManager.Instance.Subscribe(updateEventName, BuildNavMesh);
    }

    private void BuildNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh ������� ��������!");
        }
        else
        {
            Debug.LogWarning("NavMeshSurface �� ������!");
        }
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(updateEventName, BuildNavMesh);
        }
    }
}
