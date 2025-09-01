using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaiRacket : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip racketHit;
    public Transform normal;

    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball == null) return;

        audioSource.PlayOneShot(racketHit);

        ball.enabled = true;
        //ball.velocity = new Vector3(-ball.velocity.x, ball.velocity.y, ball.velocity.z);
        Vector3 normalDir = (normal.position - transform.position).normalized;
        float side = -Vector3.Dot(normalDir, ball.velocity.normalized);

        ball.velocity = ball.velocity.magnitude * normalDir * side;
    }
}