using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class PointSystem : MonoBehaviour
{
    public GameObject spaceParticles;

    public float combo = 0;
    public float score = 0;
    public TMP_Text scoreText;
    public TMP_Text scoreNum;
    public Animator scoreAnimator; 

    public static PointSystem instance;

    public Image scoreBar; 
    public Image comboBar; 

    public int maxScore = 20;

    public Wall wall;
    public Ball ball;

    public float maxWallDisplacement = 20;
    public int moveBackCombo = 5;
    public float comboMoveAmount = 2f;

    private float comboMovement = 0;

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
        if (score > 0)
        {
            SetScoreTextVisible(true); 
        }
        if (scoreAnimator != null)
        {
            //scoreAnimator.SetTrigger("pointGain");
            //was too energetic
        }

        if ((combo % 5)==0) {
            comboMovement += comboMoveAmount;
        }

        UpdateScoreUI();

        wall.offset = new Vector3(comboMovement + (score/maxScore)*maxWallDisplacement, 0, 0);

    }

    public void ResetScore() 
    {
        if(score > 0){
            score -= .5f;
        }
        combo = 0;
        UpdateScoreUI();
        SetScoreTextVisible(false); 

        comboMovement = 0;
        wall.offset = new Vector3(comboMovement  + (score/maxScore)*maxWallDisplacement, 0, 0);



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
            comboBar.fillAmount = Mathf.Clamp01((score+Mathf.Floor(combo/5)) / maxScore);
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
        Debug.Log($"Ball hit wall {id}, spaceBall is {ball.spaceBall}");
        if(!ball.spaceBall){
            AddPoint();
        }else{
            ResetScore();
        }
    }

    public void hitSpace(){
        Debug.Log($"Ball hit space , spaceBall is {ball.spaceBall}");

        if(ball.spaceBall){
            AddPoint();
            Instantiate(spaceParticles, ball.position, Quaternion.identity);
        }else{
            ResetScore();
        }

    }

    public static void HitWall(int id){
        instance.hitWall(id);
    }

    public static void HitSpace(){
        instance.hitSpace();
    }

    public static void OutOfBounds(){
        instance.ResetScore();
    }
}
