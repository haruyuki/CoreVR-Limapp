using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaiRacket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball == null) return;

        ball.enabled = true;
        ball.velocity = new Vector3(-ball.velocity.x, ball.velocity.y, ball.velocity.z);
    }
}