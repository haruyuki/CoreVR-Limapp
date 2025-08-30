using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject ball;
    public GameObject brokenParticle;
    public PointSystem pointSystem;

    [Header("Shrink / Combo Settings")]
    public float shrinkPerHit = 0.1f;
    public float minScaleFactor = 0.2f;
    private Vector3 _initialScale;
    private int _combo = 0;
    // Start is called before the first frame update
    void Start()
    {
        _initialScale = transform.localScale; // remember starting size
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (brokenParticle != null)
            Instantiate(brokenParticle, transform.position, Quaternion.identity);

        transform.position = new Vector3(transform.position.x, transform.position.y + UnityEngine.Random.Range(-1f,1f), transform.position.z + UnityEngine.Random.Range(-1f,1f));

        if (pointSystem != null)
        {
            //pointSystem.AddPoint();
            Debug.Log("plus a point");
        }

        //combo when hit will shrink
        _combo++;
        float factor = Mathf.Max(minScaleFactor, 1f - _combo * shrinkPerHit);
        transform.localScale = _initialScale * factor;

        if (ballScript != null)
        {
            ballScript.SetCombo(_combo);
        }

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
    }
}
