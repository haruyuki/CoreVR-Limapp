using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity = new Vector3(0,1,0);

    public float g = 9.1f; //gravity

    public float floorY = 0;
    public float wallZ = 10;
    public float oobZ = -10;

    public AudioClip[] floorBounceClip;
    public AudioClip[] wallBounceClip;

    private AudioSource source;

    public Transform startPos;

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

        if(position.z > wallZ)
        {
            velocity = new Vector3(velocity.x, velocity.y, -velocity.z);
            position.z = wallZ;
            //source.PlayOneShot(wallBounceClip[UnityEngine.Random.Range(0, wallBounceClip.Length-1)]);

        }

        if (position.z < oobZ)
        {
            StartCoroutine(returnToStart());

        }

        velocity = new Vector3(velocity.x, velocity.y-(Time.deltaTime*g), velocity.z);
        
        transform.position = position;

    }

    IEnumerator returnToStart()
    {
        Vector3 start = transform.position;

        float duration = 1f;
        float elapsed = 0f;

        while(elapsed < duration){
            transform.position = Vector3.Lerp(start, startPos.position, elapsed/duration);
            position = transform.position;
            elapsed += Time.deltaTime;
            yield return null;


        }

        transform.position = startPos.position;
        position = startPos.position;
        velocity = new Vector3(-20, 0, 0);
        yield return null;
        gameObject.GetComponent<Ball>().enabled = false;


    }
}
