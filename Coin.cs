using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// generic collectable resource
/// </summary>
public class Coin : MonoBehaviour {

    public int worth = 2;   //adds to player points

    public int WasScored()
    {
        Invoke( "ScoredAndDone", 0.2f );
        return worth;
    }
    void ScoredAndDone()
    {
        Destroy( this.gameObject );
    }

}
