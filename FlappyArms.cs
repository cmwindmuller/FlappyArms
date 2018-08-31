using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// arm/controller movement, tracking velocity, interpeting data for player to use
/// </summary>
public class FlappyArms : MonoBehaviour {
    /// <summary>
    /// solely finds hand velocity
    /// </summary>
    struct Arm
    {
        public Arm(int id)
        {
            this.id = id;
            velocity = pos0 = pos1 = gs.VRPlatform.GetLocalControllerPosition( id );
        }
        public int id;
        public Vector3 velocity,pos0,pos1;
        public void UpdateTracking()
        {
            pos1 = pos0;
            pos0 = gs.VRPlatform.GetLocalControllerPosition( id );
            velocity = pos0 - pos1;
        }
    }
    Arm leftArm, rightArm;
    // main scalars for adjusting output data
    public float thrustFactor=1,slideFactor=1;
    // angle and distance between hands
    float flapAngle, armSpan;
    // visual, audbile parts of flapping
    public ParticleSystem flapPfx;
    public AudioSource flapSfx;
    bool busyFx;//prevents double-starting an Fx

    void Start ()
    {
        leftArm = new Arm( 0 );
        rightArm = new Arm( 1 );
    }
	void Update ()
    {
        leftArm.UpdateTracking();
        rightArm.UpdateTracking();
        Vector3 hand2hand = rightArm.pos0 - leftArm.pos0; //basic vector between the hands
        Vector3 handsLateral = hand2hand;
        handsLateral.y = 0; //horizontal version of hand2hand
        armSpan = hand2hand.magnitude; //wide armSpan implies a stronger flap
        flapAngle = Vector3.Angle( hand2hand, handsLateral );
	}
    void AttemptFx()
    {
        if( busyFx ) return;//busy playing an fx, already
        busyFx = true;
        if( !flapPfx.isPlaying )
            flapPfx.Play();
        if( !flapSfx.isPlaying )
            flapSfx.Play();
        Invoke( "FinishFx", 0.4f );
    }
    void FinishFx()
    {
        busyFx = false;//clear for future Fx
    }
    /// <summary>
    /// downward flap scalar, non-raw
    /// </summary>
    /// <returns>usuable flap amount</returns>
    public float FlapThrustAmount()
    {
        float flapThrust = Mathf.Abs( Mathf.Min(  leftArm.velocity.y, 0 )
                                    + Mathf.Min( rightArm.velocity.y, 0 ) );
        flapThrust = 0.2f * Mathf.Pow( flapThrust / Time.deltaTime, 2 );
        if( flapThrust < 2 )
            return flapThrust = 0;
        AttemptFx();
        return flapThrust * thrustFactor * armSpan;
    }
    /// <summary>
    /// how much are the arms leaning, non-raw
    /// </summary>
    /// <returns>usuable arm angle</returns>
    public float FlapSlideAmount()
    {
        flapAngle = Mathf.Clamp01( flapAngle / 60 );//start with scaled down degrees
        if( leftArm.pos0.y < rightArm.pos0.y )//hand height informs when the angle is negative
            flapAngle *= -1;
        return flapAngle * slideFactor * armSpan;
    }
}
