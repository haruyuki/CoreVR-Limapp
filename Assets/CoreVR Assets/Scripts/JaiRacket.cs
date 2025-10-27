using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaiRacket : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip racketHit;
    public Transform normal;
    public float aimAssist = .5f;
    private float currentAimAssist = .5f;
    public float doubleHitBuffer = .1f;
    private float doubleHit = 0;

    private Vector3 lastPos;
    public Vector3 racketVelocity;

    public float velocityMultiplier = 5;

    void Update(){
        if(doubleHit > 0){
            doubleHit -= Time.deltaTime;
        }

        racketVelocity = (transform.position - lastPos);
        lastPos = transform.position;


    }

    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball == null || doubleHit > 0) return;

        doubleHit = doubleHitBuffer;

        audioSource.PlayOneShot(racketHit);

        ball.enabled = true;
        //ball.velocity = new Vector3(-ball.velocity.x, ball.velocity.y, ball.velocity.z);
        Vector3 normalDir = (normal.position - transform.position).normalized;
        float side = -Vector3.Dot(normalDir, ball.velocity.normalized);

        currentAimAssist = 0;
        if(!ball.spaceBall){
            currentAimAssist = aimAssist;
        }

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (ball.spaceBall){
            rb.useGravity = false;
        }

        ball.HitRacket();

        Vector3 towardsTarget = (PointSystem.instance.wall.transform.position + new Vector3(0, 3, 0) - ball.position).normalized;
        Vector3 hitDir = normalDir * (side > 0 ? 1 : -1);

        float velocityDot = -Vector3.Dot(racketVelocity.normalized, ball.velocity.normalized);
        Vector3 velocityVector = velocityDot*racketVelocity.magnitude*velocityMultiplier * Vector3.Scale(Vector3.Lerp(hitDir, towardsTarget, currentAimAssist).normalized, new Vector3(1,0,1));

        ball.velocity = (ball.GetSpeedVector().magnitude * Vector3.Lerp(hitDir, towardsTarget, aimAssist).normalized) + velocityVector;
        rb.useGravity = true;
    }
}