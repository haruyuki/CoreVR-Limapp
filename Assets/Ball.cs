using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity = new Vector3(0,10,0);

    public float g = 9.1f; //gravity

    public float floorY = 0;
    public float wallX = 10;
    public float oobX = -10;
    public float oobZ = 0;

    public AudioClip[] floorBounceClip;
    public AudioClip[] wallBounceClip;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        position = position + velocity*Time.deltaTime;

        if(position.y < floorY)
        {
            velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
            position.y = floorY;
            //source.PlayOneShot(floorBounceClip[UnityEngine.Random.Range(0, floorBounceClip.Length-1)]);
        }

        if(position.x > wallX)
        {
            velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
            position.x = wallX;
            //source.PlayOneShot(wallBounceClip[UnityEngine.Random.Range(0, wallBounceClip.Length-1)]);

        }

        if (position.x < oobX)
        {
            velocity = new Vector3(-velocity.x, velocity.y, velocity.z);
            position.x = oobX;

        }

        velocity = new Vector3(velocity.x, velocity.y-(Time.deltaTime*g), velocity.z);
        
        transform.position = position;

    }
}
