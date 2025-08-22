using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject ball;
    public GameObject brokenParticle;
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
        Instantiate(brokenParticle, transform.position, Quaternion.identity);
        transform.position = new Vector3(transform.position.x, transform.position.y + UnityEngine.Random.Range(-1f,1f), transform.position.z + UnityEngine.Random.Range(-1f,1f));

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
