using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.Core;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Avatars;
using Liminal.SDK.VR.Input;

public class JaiRacket : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip racketHit;
    public Transform normal;

    //aim assist lerps between these 2
    public float startAimAssist = .1f;
    public float endAimAssist = .5f;

    private float currentAimAssist = 0f;

    public float doubleHitBuffer = .1f;
    private float doubleHit = 0;

    private Vector3 lastPos;
    public Vector3 racketVelocity;

    public float velocityMultiplier = 5;

    void Update(){
        if(doubleHit > 0){
            doubleHit -= Time.deltaTime;
        }

        racketVelocity = (transform.position - lastPos);
        lastPos = transform.position;


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

        currentAimAssist = 0;
        if(!ball.isBombBall){
            currentAimAssist = Mathf.Lerp(startAimAssist, endAimAssist, PointSystem.instance.ScorePercent());
        }

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (ball.isBombBall){
            rb.useGravity = false;
        }

        ball.HitRacket();

        // var rightHandInput = GetInput(VRInputDeviceHand.Right);
        // rightHandInput?.SendInputHaptics(frequency: .5f, amplitude: .5f, duration: 0.05f);

        Vector3 towardsTarget = (PointSystem.instance.wall.transform.position + new Vector3(0, 3, 0) - ball.position).normalized;
        Vector3 hitDir = normalDir * (side > 0 ? 1 : -1);

        float velocityDot = -Vector3.Dot(racketVelocity.normalized, ball.velocity.normalized);
        Vector3 velocityVector = velocityDot*racketVelocity.magnitude*velocityMultiplier * Vector3.Scale(Vector3.Lerp(hitDir, towardsTarget, currentAimAssist).normalized, new Vector3(1,0,1));

        ball.velocity = (ball.GetTargetSpeed() * Vector3.Lerp(hitDir, towardsTarget, currentAimAssist).normalized) + velocityVector;
        rb.useGravity = true;
    }

    private IVRInputDevice GetInput(VRInputDeviceHand hand)
        {
            var device = VRDevice.Device;
            return hand == VRInputDeviceHand.Left ? device.SecondaryInputDevice : device.PrimaryInputDevice;
        }
}