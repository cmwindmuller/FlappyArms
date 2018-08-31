using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// World.movement for GameObjects
/// </summary>
public class ObjectPanner : MonoBehaviour {
	void Update () {
        transform.position -= World.movement * Time.deltaTime;
	}
}
