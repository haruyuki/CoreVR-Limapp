using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaiRacket : MonoBehaviour
{
    void Update() 
    {
        //transform.Translate(1f, 0, 0);
    }

    void OnCollisionEnter(Collision collision) 
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        
        if (ball != null) 
            {
                ball.enabled = true;
            }
    }
    
}
