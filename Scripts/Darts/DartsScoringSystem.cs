using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartScoringSystem : MonoBehaviour
{
    public GameObject dartboardCenter;   // Reference to the dartboard center as a GameObject
    public float maxScoreDistance = 1f;  // Distance for maximum score (bullseye)
    public float mediumScoreDistance = 2f; // Distance for medium score
    public float lowScoreDistance = 3f;  // Distance for low score

    private bool hasLanded = false;     // To check if dart has landed

    public GameManager gameManager;     // Reference to the GameManager for updating score

    // Use OnCollisionEnter if your dartboard's Collider is not a Trigger
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);  // Debug log to see what collided

        if (!hasLanded)
        {
            // Ensure the dart hit the dartboard
            if (collision.gameObject.tag == "Dartboard")
            {
                Debug.Log("Dart collided with Dartboard!");  // Debug log to confirm collision with dartboard

                // Get the current position of the dart when it hits
                Vector3 dartPosition = transform.position;

                // Use dartboardCenter's transform to calculate the distance
                float distance = Vector3.Distance(dartPosition, dartboardCenter.transform.position);
                Debug.Log("Distance from center: " + distance);  // Debug log to show distance

                // Calculate and update the score based on the distance
                int score = CalculateScore(distance);

                // Update the score in the game manager
                if (gameManager != null)
                {
                    gameManager.UpdateScore(score);
                }

                // Mark dart as landed so we don't recalculate
                hasLanded = true;
            }
        }
    }

    // Method to calculate score based on distance
    int CalculateScore(float distance)
    {
        int score = 0;

        if (distance <= maxScoreDistance)
        {
            score = 50;  // Bullseye
        }
        else if (distance <= mediumScoreDistance)
        {
            score = 25;  // Medium score
        }
        else if (distance <= lowScoreDistance)
        {
            score = 10;  // Low score
        }
        else
        {
            score = 0;   // Missed target
        }

        // Output score to the console for debugging
        Debug.Log("Calculated Score: " + score);

        return score;
    }
}
