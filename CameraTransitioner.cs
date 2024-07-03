using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraTransitioner : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras; // Assign all room cameras in the inspector
    public Transform player; // Assign the player transform in the inspector

    void Update()
    {
        CinemachineVirtualCamera closestCamera = null;
        float closestDistance = float.MaxValue;

        // Determine the closest camera based on player position
        foreach (CinemachineVirtualCamera cam in cameras)
        {
            float distance = Vector3.Distance(player.position, cam.transform.position);
            if (distance < closestDistance)
            {
                closestCamera = cam;
                closestDistance = distance;
            }
        }

        // Prioritize the closest camera
        if (closestCamera != null)
        {
            foreach (CinemachineVirtualCamera cam in cameras)
            {
                cam.Priority = (cam == closestCamera) ? 10 : 0;
            }
        }
    }
}
