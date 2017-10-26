/*
 * Joe O'Regan
 * K00203642
 * 
 * PlayerController.cs
 * Space Shooter Unity Tutorial
 * 
 * Handle player movement and firing
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveMeneuver : MonoBehaviour {

    public float dodge;             // help us pick target maneuver
    public float smoothing;         // smoothing time for newManeuver
    public float tilt;              // Tilt the enemy ship similar to the Player
    public Vector2 startWait;       // Wait before firing
    public Vector2 maneuverTime;    // How long is it doing the maneuver for
    public Vector2 meneuverWait;    // Wait time before new maneuver
    public Boundary boundary;

    private float currentSpeed;     
    private float targetManeuver;
    private Rigidbody rb;
        
	void Start () {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.z;
        StartCoroutine(Evade());
	}
    

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);                    // Mathf.Sign reverses the sign, so Enemy ship dodges towards the center
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(meneuverWait.x, meneuverWait.y));
        }
    }

	void FixedUpdate () {
        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);   // moving rigidbody velocity x, towards target maneuver, with deltaTime speed
        rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);                                         // Always moving at same speed
        rb.position = new Vector3// Clamp the enemy rigidbody position
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
            0.0f, 
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);                                  // Tilt the Enemy Ship
	}
}
