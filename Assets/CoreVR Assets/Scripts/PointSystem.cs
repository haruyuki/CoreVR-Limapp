using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointSystem : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText;

    public static PointSystem instance;
   
    void Start()
    {
        UpdateScoreUI();
        instance = this;
    }

    public void AddPoint()
    {
        score++;
        UpdateScoreUI();
    }

    public void ResetScore() //call this when we add the losing mechanic
    {
        score = 0;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if(scoreText !=null)
        {
            scoreText.text = "" + score;
        }
    }
}
