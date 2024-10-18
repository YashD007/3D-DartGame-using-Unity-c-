using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dartboard : MonoBehaviour
{
    public int outerBullseyeScore = 25;
    public int innerBullseyeScore = 50;
    public int[] segmentScores = new int[20] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
    public int tripleMultiplier = 3;
    public int doubleMultiplier = 2;

    private int totalScore = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dart"))
        {
            Dart dart = other.GetComponent<Dart>();

            if (dart != null)
            {
                // Determine score based on the hit zone
                switch (dart.hitZone)
                {
                    case "InnerBullseye":
                        AddScore(innerBullseyeScore);
                        break;
                    case "OuterBullseye":
                        AddScore(outerBullseyeScore);
                        break;
                    case "Triple":
                        AddScore(segmentScores[dart.segmentIndex] * tripleMultiplier);
                        break;
                    case "Double":
                        AddScore(segmentScores[dart.segmentIndex] * doubleMultiplier);
                        break;
                    case "Single":
                        AddScore(segmentScores[dart.segmentIndex]);
                        break;
                    default:
                        Debug.LogWarning("Unknown hit zone!");
                        break;
                }
            }
        }
    }

    void AddScore(int score)
    {
        totalScore += score;
        Debug.Log("Total Score: " + totalScore);
    }
}

