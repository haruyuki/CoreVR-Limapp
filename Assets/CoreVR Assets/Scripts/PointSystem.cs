using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointSystem : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText;
    public TMP_Text scoreNum;
    public Animator scoreAnimator; 

    public static PointSystem instance;
   
    void Start()
    {
        instance = this;
        UpdateScoreUI();
        SetScoreTextVisible(false); 
    }

    public void AddPoint()
    {
        score++;
        UpdateScoreUI();
        if (score > 0)
        {
            SetScoreTextVisible(true); 
        }
        if (scoreAnimator != null)
        {
            //scoreAnimator.SetTrigger("pointGain");
            //was too energetic
        }
    }

    public void ResetScore() 
    {
        score = 0;
        UpdateScoreUI();
        SetScoreTextVisible(false); 
    }

    void UpdateScoreUI()
    {
        if(scoreNum !=null)
        {
            scoreNum.text = "x" + score;
        }
    }

    void SetScoreTextVisible(bool visible)
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(visible);
        }
        if (scoreNum != null)
        {
            scoreNum.gameObject.SetActive(visible);
        }
    }
}
