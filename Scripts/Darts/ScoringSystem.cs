using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    // Reference to the target (e.g., dartboard center or bullseye)
    public Transform target;

    // Reference to the player or the object whose score you want to calculate
    public Transform player;

    // Maximum distance at which scoring begins
    public float maxDistance = 10.0f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the player and the target using Vector3.magnitude
        float distance = (player.position - target.position).magnitude;

        // Calculate score based on the distance
        int score = CalculateScore(distance);

        // Display or use the score
        Debug.Log("Score: " + score);
    }

    // Function to calculate score based on the distance
    int CalculateScore(float distance)
    {
        // If distance is greater than the maximum, no score
        if (distance > maxDistance)
        {
            return 0;
        }

        // Example scoring system: closer distance = higher score
        // You can customize the scoring formula
        float normalizedDistance = Mathf.Clamp01(1 - (distance / maxDistance)); // Closer to 1 if near
        int score = Mathf.RoundToInt(normalizedDistance * 100); // Score from 0 to 100
        return score;
    }
}

