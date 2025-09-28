using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class PointSystem : MonoBehaviour
{
    public float combo = 0;
    public float score = 0;
    public TMP_Text scoreText;
    public TMP_Text scoreNum;
    public Animator scoreAnimator; 

    public static PointSystem instance;

    public Image scoreBar; 
    public int maxScore = 20;

    public Transform[] walls;

    void Start()
    {
        instance = this;
        UpdateScoreUI();
        SetScoreTextVisible(false); 
    }

    public void AddPoint()
    {
        combo++;
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
        if(score > 0){
            score -= 1;
        }
        combo = 0;
        UpdateScoreUI();
        SetScoreTextVisible(false); 
    }

    void UpdateScoreUI()
    {
        if(scoreNum != null)
        {
            scoreNum.text = "x" + combo;
        }
        if(scoreBar != null && maxScore > 0)
        {
            scoreBar.fillAmount = Mathf.Clamp01((float)score / maxScore);
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

    public void hitWall(int id){
        Debug.Log($"Ball hit wall {id}");
    }

    public static void HitWall(int id){
        instance.hitWall(id);
    }
}
