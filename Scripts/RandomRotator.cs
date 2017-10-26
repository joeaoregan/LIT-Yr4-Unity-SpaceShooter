/*
 * 20/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * RandomRotator.cs
 * 
 * Randomly rotate asteroids
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {

    private Rigidbody rb;
    public float tumble;                                        // Tumble speed to be set in Unity
    
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere * tumble;  // Angular velocity: how fast a body is rotating, insideUnitSphere has random vector3 value for X, Y, and Z
	}	
}
