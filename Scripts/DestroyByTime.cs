/*
 * 20/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * DestroyByTime.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {
    public float lifetime;                      // Wait before object is destroyed

	void Start () {
        Destroy(gameObject, lifetime);          // When the gameObject is instantiated it will destroy itself when time is up
	}	
}
