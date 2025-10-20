using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    public float spaceDistance = 25;

    public Vector3 position;
    public Vector3 velocity = new Vector3(0,10,0);
    public Vector3 startVelocity;
    public static event System.Action OnMissed;

    public float g = 9.1f; //gravity

    public float floorY = 0;
    public GameObject floorHitParticle;
    public float oobX = -10;
    public float oobZ = 0;

    public float ballSpreadHeightStart = 4;
    public Vector2 ballSpread = new Vector2(2,1); //height, width

    public AudioClip[] floorBounceClip;
    public AudioClip[] wallBounceClip;
    public AudioClip[] targetClip;

    public TrailRenderer trailRenderer;

    private AudioSource source;

    public Transform startPos;

    //combo speed
    [Header("Combo Speed")]
    private float baseSpeed = 0f;

    [Tooltip("Extra speed added per combo hit.")]
    public float extraSpeedPerCombo = 50.0f;

    private int _currentCombo = 0;

    public GameObject ball;
    private GameObject blueBall;
    private GameObject greenBall;

    public bool spaceBall = false;


   
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        baseSpeed = velocity.magnitude;

        position = transform.position;
        startVelocity = velocity;

        blueBall = ball.transform.GetChild(0).gameObject;
        greenBall = ball.transform.GetChild(1).gameObject;
        ChooseBall();
    }

    // Update is called once per frame
    void Update()
    {
        position = position + velocity*Time.deltaTime;

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

        velocity = new Vector3(velocity.x, velocity.y-(Time.deltaTime*g), velocity.z);
        
        transform.position = position;

    }

    private void ChooseBall() {

        float chance = 0.1f;

        //int choice = Random.Range(0f,2);

        if (Random.value < chance) {
            blueBall.SetActive(true);
            greenBall.SetActive(false);
            spaceBall = true;
            trailRenderer.startColor = new Color(1,0,0);
            chance = 0;
        } else {
            greenBall.SetActive(true);
            blueBall.SetActive(false);
            spaceBall = false;
            trailRenderer.startColor = new Color(0,1,0);
            chance += 0.1f;
        }
    }

    public void ResetBall(){
            ChooseBall();
            trailRenderer.Clear();
            position = startPos.position;
            transform.position = position;
            trailRenderer.time = .5f;

            velocity = startVelocity;
            //Target.instance.ResetComboAndSize();
            trailRenderer.Clear();
            bounces = 0;

    }


    public int maxBounces = 4;
    private float bounces = 0;
    public void HitFloor(){
        velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
        position.y = floorY;
        source.PlayOneShot(floorBounceClip[UnityEngine.Random.Range(0, floorBounceClip.Length-1)]);
        GameObject fp = Instantiate(floorHitParticle, transform.position, floorHitParticle.transform.rotation);
        fp.SetActive(true);
        bounces += 1;
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

        velocity = towardsStart * startVelocity.magnitude;

        source.PlayOneShot(wallBounceClip[UnityEngine.Random.Range(0, wallBounceClip.Length-1)]);


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


    public Vector3 GetSpeedVector(){

        Vector3 dir = velocity.normalized;
        float targetSpeed = Mathf.Max(0f, baseSpeed + _currentCombo * extraSpeedPerCombo);
        return dir * targetSpeed;
    }

   

}

