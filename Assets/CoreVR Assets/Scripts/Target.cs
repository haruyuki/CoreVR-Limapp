using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Target : MonoBehaviour
{
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

    public static Target instance;

    // Start is called before the first frame update
    void Start()
    {
        _initialScale = transform.localScale; // remember starting size
        instance = this;
        wallResetPos = wall.position;
        wallPos = wallResetPos;
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        wall.position = Vector3.Lerp(wall.position, wallPos, Time.deltaTime*wallSpeedFactor);
    }

    void OnEnable()
    {
        Ball.OnMissed += HandleBallMissed;
    }

    void OnDisable()
    {
        Ball.OnMissed -= HandleBallMissed;
    }

    private void HandleBallMissed()
    {
        ResetComboAndSize();
    }


    void BreakTarget(Ball ballScript)
    {
        if (brokenParticle != null){
            GameObject bp = Instantiate(brokenParticle, transform.position, brokenParticle.transform.rotation);
            bp.SetActive(true);
        }



        if (pointSystem != null)
        {
            pointSystem.AddPoint();
            Debug.Log("plus a point");
        }

        //combo when hit will shrink
        _combo++;
        float factor = Mathf.Max(minScaleFactor, 1f - _combo * shrinkPerHit);
        transform.localScale = new Vector3(_initialScale.x * factor, _initialScale.y, _initialScale.z * factor);


        if (ballScript != null)
        {
            ballScript.SetCombo(_combo);

        }
        wallPos += wallOffset;
        StartCoroutine(respawn());
        transform.GetComponent<MeshCollider>().enabled = (false);
        transform.GetComponent<Renderer>().enabled = (false);



    }

    IEnumerator respawn(){
        float duration = .5f;
        float elapsed = 0f;

        while(elapsed < duration){
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, startPos.y + UnityEngine.Random.Range(-.5f,1f), startPos.z + UnityEngine.Random.Range(-1f,1f));
                yield return null;

        transform.GetComponent<MeshCollider>().enabled = (true);
        transform.GetComponent<Renderer>().enabled = (true);

    }


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        var ballScript = other.GetComponentInParent<Ball>();
        if (ballScript != null)
        {
            BreakTarget(ballScript);
        }
    }
    
    //Miss/Fail/Restore size at the end of the round
    public void ResetComboAndSize()
    {
        _combo = 0;
        transform.localScale = _initialScale;
        //wall.position = wallResetPos;
        wallPos = wallResetPos;
        
    }
}
