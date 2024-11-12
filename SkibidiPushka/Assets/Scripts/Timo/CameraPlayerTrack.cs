using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerTrack : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField, Range(-1,-100)] float zOffset = -10f;
    [SerializeField, Range(1, 100)] float cameraSpeed = 10f;
    private Vector3 _previousPosition;

    private void Start()
    {
        var cameraStartPosition = player.transform.position;
        cameraStartPosition.z = zOffset;
        Camera.main.transform.position = cameraStartPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        var currPosition = player.transform.position;
        currPosition.z = zOffset;
        currPosition.x = Mathf.Lerp(_previousPosition.x, currPosition.x, cameraSpeed*Time.deltaTime);
        currPosition.y = Mathf.Lerp(_previousPosition.y, currPosition.y, cameraSpeed * Time.deltaTime);
        transform.position = currPosition;
        _previousPosition = transform.position;
    }
}
