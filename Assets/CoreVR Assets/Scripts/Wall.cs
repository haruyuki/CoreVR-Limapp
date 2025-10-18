using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Wall : MonoBehaviour
{
    public int id = 0;

    public GameObject ball;
    public GameObject brokenParticle;
    public PointSystem pointSystem;


    public Transform wall;
    public Vector3 wallPos;
    public Vector3 wallOffset;

    public float wallSpeedFactor;
    private static Vector3 wallResetPos;


    [Header("Shrink / Combo Settings")]
    public float shrinkPerHit = 0.1f;
    public float minScaleFactor = 0.2f;
    private Vector3 _initialScale;
    private int _combo = 0;
    private Vector3 startPos;

    public static Wall instance;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        _initialScale = transform.localScale; // remember starting size
        wallResetPos = wall.position;
        wallPos = wallResetPos;
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        wall.position = Vector3.Lerp(wall.position, wallPos, Time.deltaTime*wallSpeedFactor);
    }

    


    void BreakTarget(Ball ballScript)
    {
        if (brokenParticle != null){
            GameObject bp = Instantiate(brokenParticle, transform.position, brokenParticle.transform.rotation);
            bp.SetActive(true);
        }



        StartCoroutine(respawn());
        //transform.GetComponent<MeshCollider>().enabled = (false);
        transform.GetComponent<Renderer>().enabled = (false);



    }

    IEnumerator respawn(){
        float duration = .5f;
        float elapsed = 0f;

        while(elapsed < duration){
            elapsed += Time.deltaTime;
            yield return null;
        }


        //transform.GetComponent<MeshCollider>().enabled = (true);
        transform.GetComponent<Renderer>().enabled = (true);

    }


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        var ballScript = other.GetComponentInParent<Ball>();
        if (ballScript != null)
        {
            /*
            //the matching logic
            bool correctWall = 
                (ballScript.currentColor == Ball.BallColor.Green && id == 1) ||
                (ballScript.currentColor == Ball.BallColor.Blue && id == 0);

            if (correctWall)
            {
                BreakTarget(ballScript);
                ballScript.HitWall();
                PointSystem.HitWall(id);
            }
            else
            {
                //wrong wall boi
                ResetComboAndSize();
                ballScript.ResetCombo();
                ballScript.HitWall();
                PointSystem.HitWall(id);


            }*/
            BreakTarget(ballScript);
            PointSystem.HitWall(id);
            ballScript.HitWall();

        }
    }

    public void AddPoint(){
        _combo++;

        Debug.Log("Combo:" + _combo + "Combo % 5:" + (_combo%5));


        if ((_combo % 5)==0) {
            wallPos += wallOffset;
        }

       

    }
    
    //Miss/Fail/Restore size at the end of the round
    public void ResetScore()
    {
        _combo = 0;
        //wall.position = wallResetPos;
        wallPos = wallResetPos;

        
    }
}
