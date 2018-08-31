using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player has Y and X movement, but Z is faked by World.movement
/// </summary>
public class FlappyPlayer : MonoBehaviour {
    //this players arms and physics component, ensured on Start
    FlappyArms arms;
    Rigidbody rbody;

    public bool hasStarted; // freeze player until they're ready
    Vector3 movement;   // passes info to World
    public float forwardSpeed;
    // speed limits, falling and climbing are isolated
    public float maxUpSpeed,maxDownSpeed,maxSideSpeed;
    // gameScore
    public int points;
    const string key_score = "Flap_Arms_TopScore";//save key
    // display score, play sound
    public Text scoreText;
    public AudioSource scoreSfx;

    void Start ()
    {
        arms = gameObject.GetComponent<FlappyArms>();
        if( !arms )
            arms = gameObject.AddComponent<FlappyArms>();
        rbody = gameObject.GetComponent<Rigidbody>();
        if( !rbody )
            rbody = gameObject.AddComponent<Rigidbody>();

        hasStarted = false;// wait for player button
        rbody.isKinematic = true;
        movement.z = forwardSpeed;
    }
    void Update()
    {
        if(!hasStarted && gs.VRPlatform.AButtonDown)
        {
            hasStarted = true;
            rbody.isKinematic = false;
        }
    }
    /// <summary>
    /// wait for all other updates, then move the player, going last is mildly safer
    /// </summary>
    void LateUpdate () {
        float thrust = arms.FlapThrustAmount();
        float slide = arms.FlapSlideAmount();
        //at last, we finally Flap!
        rbody.AddForce( Vector3.up * thrust + Vector3.right * slide );
        // prevent any strange bouncing or velocity problems when grounded
        if( transform.position.y < 0)
        {
            transform.position = new Vector3( transform.position.x, 0, transform.position.z );
            rbody.velocity = Vector3.zero;
        }
        else
        {
            // Y speed limit and X speed limit do not affect another
            if( rbody.velocity.y > maxUpSpeed )
            {
                Vector3 velo = rbody.velocity;
                velo.y = maxUpSpeed;
                rbody.velocity = velo;
            }
            else if( rbody.velocity.y < -maxDownSpeed )
            {
                Vector3 velo = rbody.velocity;
                velo.y = -maxDownSpeed;
                rbody.velocity = velo;
            }
            if(Mathf.Abs( rbody.velocity.x ) > maxSideSpeed )
            {
                Vector3 velo = rbody.velocity;
                velo.x = maxSideSpeed * Mathf.Sign( velo.x );
                rbody.velocity = velo;
            }
        }
        // atm, faux movment is only in the Z axis
        World.movement = movement;
	}
    private void OnTriggerEnter( Collider other )
    {
        Coin c = other.gameObject.GetComponent<Coin>();
        if(c)// it was a coin!
        {
            points += c.WasScored();// beginning of the end of the coin
            scoreText.text = points.ToString();
            scoreSfx.Play();
        }
    }
}
