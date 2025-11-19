using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    public float spaceDistance = 25;

    [Header("Player proximity slowdown")]

    [Tooltip("Distance at which the slowdown begins")]
    public float playerSlowownDistance = 4;

    [Tooltip("Slowdown at score = 0")]
    public float playerSlowdownStartPercent = 1f;

    [Tooltip("Slowdown at score = maxScore")]
    public float playerSlowdownEndPercent = .4f;

    [Header("Physics attributes")]

    public Vector3 position;
    public Vector3 velocity = new Vector3(0,10,0);
    private Vector3 startVelocity;

    public float maxYVeloctity = 10;
    public static event System.Action OnMissed;

    public float g = 9.1f; //gravity

    private float floorY = 0; //y position of the ground

    public AnimationCurve repeatedBounceBoost;
    public float distanceBounceScale = 2;


    public float oobX = -10;
    public float oobZ = 0;


    [Header("Height Limiting")]

    public float maxHeight = 2f;
    public float heightLimitForce = 1f;

    [Header("Return perameters")]

    public float ballSpreadHeightStart = 4;
    public Vector2 ballSpread = new Vector2(2,1); //height, width

    [Header("Effects")]

    public GameObject floorHitParticle;


    public AudioClip[] floorBounceClip;
    public AudioClip[] wallBounceClip;
    public AudioClip[] targetClip;

    public TrailRenderer trailRenderer;

    public AudioSource source;

    public Transform startPos;

    //combo speed
    [Header("Ball Speedup")]
    public float baseSpeed = 10f;

    public float endSpeed = 21f;


    public GameObject ball;
    private GameObject bombBall;
    private GameObject greenBall;

    public GameObject boundary;

    public bool isBombBall = false;


   
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        baseSpeed = velocity.magnitude;

        position = transform.position;
        startVelocity = velocity;

        bombBall = ball.transform.GetChild(0).gameObject;
        greenBall = ball.transform.GetChild(1).gameObject;
        ChooseBall();
    }

    // Update is called once per frame
    void Update()
    {
        float timeScale = 1f;
        float dist = Vector3.Distance(Vector3.zero, position);
        if(dist < playerSlowownDistance){
            float playerSlowdownPercent =  Mathf.Lerp(playerSlowdownStartPercent, playerSlowdownEndPercent, PointSystem.instance.ScorePercent());
            timeScale = Mathf.Lerp(playerSlowdownPercent, 1, dist/playerSlowownDistance);

        }

        position = position + velocity*Time.deltaTime*timeScale;

        //height limiting
        if(position.y >= maxHeight && velocity.y > 0){
            Debug.Log("Limiting Height");
            velocity.y -= heightLimitForce * Time.deltaTime * timeScale;
        }



        if(position.y < floorY)
        {
           HitFloor();
        }

        /*
        if(position.x > wall.transform.position.x)
        {
           HitWall();

        }
        */
        if(position.magnitude > spaceDistance){
            position = position.normalized * spaceDistance;

            PointSystem.HitSpace();
            ResetBall();

        }

        if (position.x < oobX)
        {
            //PointSystem.instance.ResetScore();
            ResetBall();
            PointSystem.OutOfBounds();
        }

        if (Mathf.Abs(position.z) > oobZ)
        {
            //PointSystem.instance.ResetScore();
            ResetBall();
            PointSystem.OutOfBounds();
        }

        if (boundary)
        {
            //PointSystem.instance.ResetScore();
            ResetBall();
            PointSystem.OutOfBounds();
        }

        if(isBombBall && lastHitRacket){
            //remove gravity
        }else{
            velocity = new Vector3(velocity.x, velocity.y-(Time.deltaTime*g*timeScale), velocity.z);
        }
        transform.position = position;

    }

    private void ChooseBall() {

        float chance = 0.2f;

        //int choice = Random.Range(0f,2);

        if (Random.value < chance) {
            bombBall.SetActive(true);
            greenBall.SetActive(false);
            isBombBall = true;
            trailRenderer.startColor = new Color(1,0,0);
            chance = 0;
        } else {
            greenBall.SetActive(true);
            bombBall.SetActive(false);
            isBombBall = false;
            trailRenderer.startColor = new Color(0,1,0);
            chance += 0.1f;
        }

    }

    public void ResetBall(){
            ChooseBall();
            g = 9.1f;
            trailRenderer.Clear();
            position = startPos.position;
            transform.position = position;
            trailRenderer.time = .5f;

            velocity = startVelocity;
            //Target.instance.ResetComboAndSize();
            trailRenderer.Clear();
            bounces = 0;
            lastHitRacket = false;


    }


    public int maxBounces = 4;
    private float bounces = 0;
    private float lastBounce = 0;
    private bool lastHitRacket = false;

    public void HitRacket(){
        lastHitRacket = true;
    }

    public void HitFloor(){
        velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
        position.y = floorY;
        source.PlayOneShot(floorBounceClip[UnityEngine.Random.Range(0, floorBounceClip.Length-1)]);
        GameObject fp = Instantiate(floorHitParticle, transform.position, floorHitParticle.transform.rotation);
        fp.SetActive(true);
        bounces += 1;

        float timeSinceLastBounce = (Time.fixedTime-lastBounce)/(1+(PointSystem.instance.ScorePercent() * distanceBounceScale));
        float bounceIncrease = repeatedBounceBoost.Evaluate(timeSinceLastBounce);
        if(!lastHitRacket) {velocity.y += bounceIncrease;}
        Debug.Log($"Ball Bounce, time since last bounce {timeSinceLastBounce}, ball velocity y increased by {bounceIncrease}");
        lastBounce = Time.fixedTime;
        lastHitRacket = false;
        if(Mathf.Abs(velocity.y) > maxYVeloctity){velocity.y = maxYVeloctity;}
        if(bounces > maxBounces){ ResetBall();}
    }

    public void HitWall(){
        ChooseBall();
        bounces = 0;
        //select new position
        Vector3 towardsPos = new Vector3(0,ballSpreadHeightStart+UnityEngine.Random.Range(0, ballSpread.x),(position.z>0) ? -ballSpread.y/2 : ballSpread.y/2);
        Vector3 towardsStart = (towardsPos - new Vector3(position.x, position.y, position.z)).normalized;

        if(velocity.y < 0){
            towardsStart.y *= -1;
        }

        velocity = towardsStart * GetTargetSpeed();

        source.PlayOneShot(wallBounceClip[UnityEngine.Random.Range(0, wallBounceClip.Length-1)]);

        lastHitRacket = false;


        //position.x = wall.transform.position.x;
/*
        if(hasHitTarget){
            hasHitTarget = false;
            //PointSystem.instance.AddPoint();
            trailRenderer.time += .2f;
            source.PlayOneShot(targetClip[UnityEngine.Random.Range(0, targetClip.Length-1)]);

        }else{

            //PointSystem.instance.ResetScore();
           // Target.instance.ResetComboAndSize();
            source.PlayOneShot(wallBounceClip[UnityEngine.Random.Range(0, wallBounceClip.Length-1)]);


        }
*/
    }


    public bool hasHitTarget = false;


    public float GetTargetSpeed(){

        float targetSpeed = Mathf.Lerp(baseSpeed, endSpeed, PointSystem.instance.ScorePercent());
        return targetSpeed;
    }

   

}

