using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public PointSystem pointSystem;
    
    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (!ball.isBombBall) {
            pointSystem.ResetScore();
            ball.ResetBall();
        } else {
            ball.g = 0;
        }
    }
}
