using UnityEngine;
using System.Collections;

/// <summary>
/// Uses the World.movement to animate UVs, suggesting movement
/// </summary>
public class TexturePanner : MonoBehaviour
{
    // sets U and V tiling
    public Vector2 tileScale;
    // for additional slowing/speeding
    public float movementScale;
    Vector2 uvOffset;

    void Awake()
    {
        uvOffset = new Vector2( 0, 0 );
    }

    void LateUpdate()
    {
        uvOffset.x -= World.movement.x
                                * Time.deltaTime
                                * tileScale.x
                                * movementScale
                                / transform.localScale.x;
        uvOffset.y -= World.movement.z
                                * Time.deltaTime
                                * tileScale.y
                                * movementScale
                                / transform.localScale.y;
        GetComponent<Renderer>().material.SetTextureOffset( "_MainTex", uvOffset );
    }
}