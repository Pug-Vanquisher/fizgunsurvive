using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.Events;

public class NavMeshUpdater : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    

    private void Start()
    {
        if (navMeshSurface == null)
        {
            navMeshSurface = FindObjectOfType<NavMeshSurface>();
        }

        BuildNavMesh();

        
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

}
