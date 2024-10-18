using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHitPosition : MonoBehaviour
{
    public Transform circleCenter; // Assign the center of your circle in the Inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is the circle
                if (hit.transform == transform)
                {
                    Vector3 hitPosition = hit.point;
                    Debug.Log("Hit Position: " + hitPosition);

                    // Calculate distance from the center of the circle
                    float distanceFromCenter = Vector3.Distance(hitPosition, circleCenter.position);
                    Debug.Log("Distance from Center: " + distanceFromCenter);
                }
            }
        }
    }
}