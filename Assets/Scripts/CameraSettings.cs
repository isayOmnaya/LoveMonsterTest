using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    float _spawnPointOffset = 0.0f;
    
    public void InitializeSpawnPoint(Transform spawnPoint)
    {
        if(spawnPoint != null)
        {
            Camera mainCamera = Camera.main;

            if(mainCamera != null)
            {
                // Set the spawn point to the left of the camera
                float leftBound = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x + _spawnPointOffset;

                spawnPoint.position = new Vector3(leftBound, spawnPoint.position.y, spawnPoint.position.z);
            }
            else
            {
                Debug.LogError("Main camera not found.");
            }
        }
        else
        {
            Debug.LogError("Spawn point not assigned.");
        }
    }
}
