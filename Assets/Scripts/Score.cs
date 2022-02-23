using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Text txtScore;
    int score = 0;

    void Awake() => txtScore = GetComponent<Text>();

    public void AddPoints(int points)
    {
        score += points;
        txtScore.text = $"Score : {score} pts";
    }
}
