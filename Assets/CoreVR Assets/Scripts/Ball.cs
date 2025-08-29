using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity = new Vector3(0,10,0);
    public static event System.Action OnMissed;

    public float g = 9.1f; //gravity

    public float floorY = 0;
    public float wallX = 10;
    public float oobX = -10;
    public float oobZ = 0;

    public AudioClip[] floorBounceClip;
    public AudioClip[] wallBounceClip;

    private AudioSource source;

    public Transform startPos;

    //combo speed
    [Header("Combo Speed")]
    public float baseSpeed = 0f;

    [Tooltip("Extra speed added per combo hit.")]
    public float extraSpeedPerCombo = 50.0f;

    private int _currentCombo = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        if (baseSpeed <= 0f)
            baseSpeed = velocity.magnitude;

        position = transform.position;
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
        ResetCombo();
        OnMissed?.Invoke();
        yield return null;
        gameObject.GetComponent<Ball>().enabled = false;

    }

    //Combo speed
    public void SetCombo(int combo)
    {
        _currentCombo = Mathf.Max(0, combo);
        ApplyComboSpeed();
    }

    public void ResetCombo()
    {
        _currentCombo = 0;
        ApplyComboSpeed();
    }

    private void ApplyComboSpeed()
    {
        Vector3 dir = (velocity.sqrMagnitude > 0.0001f) ? velocity.normalized : Vector3.left;
        float targetSpeed = Mathf.Max(0f, baseSpeed + _currentCombo * extraSpeedPerCombo);
        velocity = dir * targetSpeed;
    }

}

