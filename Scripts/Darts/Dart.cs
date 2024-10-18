using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    public string hitZone; // E.g., "InnerBullseye", "OuterBullseye", "Triple", "Double", "Single"
    public int segmentIndex; // Index of the segment (1 to 20)

    private void OnCollisionEnter(Collision collision)
    {
        // Logic to determine which zone was hit
        // Example: Based on the position of the dart, you can set hitZone and segmentIndex
    }
}

