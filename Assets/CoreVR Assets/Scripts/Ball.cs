using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity = new Vector3(0,10,0);
    private Vector3 startVelocity;
    public static event System.Action OnMissed;

    public float g = 9.1f; //gravity

    public float floorY = 0;
    public GameObject floorHitParticle;
    public GameObject wall;
    public float oobX = -10;
    public float oobZ = 0;

    public Vector2 ballSpread = new Vector2(2,1); //height, width

    public AudioClip[] floorBounceClip;
    public AudioClip[] wallBounceClip;
    public AudioClip[] targetClip;

    public TrailRenderer trailRenderer;


    private AudioSource source;

    public Transform startPos;

    //combo speed
    [Header("Combo Speed")]
    public float baseSpeed = 0f;

    [Tooltip("Extra speed added per combo hit.")]
    public float extraSpeedPerCombo = 50.0f;

    private int _currentCombo = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        if (baseSpeed <= 0f)
            baseSpeed = velocity.magnitude;

        position = transform.position;
        startVelocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        position = position + velocity*Time.deltaTime;

        if(position.y < floorY)
        {
           HitFloor();
        }

        if(position.x > wall.transform.position.x)
        {
           HitWall();

        }

        if (position.x < oobX)
        {
            //StartCoroutine(returnToStart());
            PointSystem.instance.ResetScore();
            //transform.position = startPos;
                        trailRenderer.Clear();

            position = startPos.position;
                    transform.position = position;
                    trailRenderer.time = .5f;


            velocity = startVelocity;
            Target.instance.ResetComboAndSize();
            trailRenderer.Clear();




        }

        if (Mathf.Abs(position.z) > oobZ)
        {
            //StartCoroutine(returnToStart());
            PointSystem.instance.ResetScore();
            //transform.position = startPos;
                        trailRenderer.Clear();
                        trailRenderer.time = .5f;

            position = startPos.position;
            transform.position = position;

            velocity = startVelocity;
            Target.instance.ResetComboAndSize();
            trailRenderer.Clear();




        }

        velocity = new Vector3(velocity.x, velocity.y-(Time.deltaTime*g), velocity.z);
        
        transform.position = position;

    }



    public void HitFloor(){
        velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
        position.y = floorY;
        source.PlayOneShot(floorBounceClip[UnityEngine.Random.Range(0, floorBounceClip.Length-1)]);
        GameObject fp = Instantiate(floorHitParticle, transform.position, floorHitParticle.transform.rotation);
        fp.SetActive(true);
    }

    public void HitWall(){

        //select new position
        Vector3 towardsPos = new Vector3(0,4+UnityEngine.Random.Range(0, ballSpread.x),UnityEngine.Random.Range(-ballSpread.y/2, ballSpread.y/2));
        Vector3 towardsStart = (towardsPos - new Vector3(position.x, position.y, position.z)).normalized;
        Vector3 newVelocity = towardsStart * velocity.magnitude;

        velocity = new Vector3(newVelocity.x, newVelocity.y, newVelocity.z);


        position.x = wall.transform.position.x;

        if(hasHitTarget){
            hasHitTarget = false;
            PointSystem.instance.AddPoint();
            trailRenderer.time += .2f;
            source.PlayOneShot(targetClip[UnityEngine.Random.Range(0, targetClip.Length-1)]);

        }else{

            PointSystem.instance.ResetScore();
            Target.instance.ResetComboAndSize();


        }
        source.PlayOneShot(wallBounceClip[UnityEngine.Random.Range(0, wallBounceClip.Length-1)]);

    }


    public bool hasHitTarget = false;

    //Combo speed
    public void SetCombo(int combo)
    {
        _currentCombo = Mathf.Max(0, combo);
        ApplyComboSpeed();
        hasHitTarget = true;
    }

    public void ResetCombo()
    {
        _currentCombo = 0;
        ApplyComboSpeed();
    }

    private void ApplyComboSpeed()
    {
        Vector3 dir = (velocity.sqrMagnitude > 0.0001f) ? velocity.normalized : Vector3.left;
        float targetSpeed = Mathf.Max(0f, baseSpeed + _currentCombo * extraSpeedPerCombo);
        velocity = dir * targetSpeed;
    }

}

