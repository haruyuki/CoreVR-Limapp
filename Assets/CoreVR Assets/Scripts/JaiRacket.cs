using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaiRacket : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip racketHit;
    public Transform normal;
    public Transform target;
    public float aimAssist = .5f;
    public float doubleHitBuffer = .1f;
    private float doubleHit = 0;

    void Update(){
        if(doubleHit > 0){
            doubleHit -= Time.deltaTime;
        }
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

        Vector3 towardsTarget = (target.position + new Vector3(0, 3, 0) - ball.position).normalized;
        Vector3 hitDir = normalDir * (side > 0 ? 1 : -1);


        ball.velocity = ball.GetSpeedVector().magnitude * Vector3.Lerp(hitDir, towardsTarget, aimAssist).normalized;
    }
}