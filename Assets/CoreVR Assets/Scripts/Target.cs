using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject ball;
    public GameObject brokenParticle;
    public PointSystem pointSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BreakTarget()
    {
        Instantiate(brokenParticle, transform.position, transform.rotation);
        transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-1f,1f), transform.position.y + UnityEngine.Random.Range(-1f,1f), transform.position.z);

        if (pointSystem != null)
        {
            pointSystem.AddPoint();
            Debug.Log("plus a point");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
       if(other.gameObject == ball)
        {

            BreakTarget();
        }
    }
}
