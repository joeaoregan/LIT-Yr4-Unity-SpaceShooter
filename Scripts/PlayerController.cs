/*
 * 20/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * PlayerController.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boundary class, does not inherit, and is serialised (needs to be serialised to view in inspector)
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;                                    // Clamp our position between the values set for min and max on x and y axes
}

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    public float speed;                                                     // Control the speed. Allow the ship to move more than 1 unit per second
    public float tilt;                                                      // Add bank/tilt to the player ship, so it rotates on x axis
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;

    public float fireRate;                                                  // Time gap between firing bullets
    private float nextFire;

    private AudioSource audioSource;                                        // Add Player Weapon sound effect

    private void Start()
    {
        rb = GetComponent<Rigidbody>();                                     // Unity 5 can no longer access components using shorthand helper references
        audioSource = GetComponent<AudioSource>();                          // Same for Audio
    }

    private void Update()
    {
        //Instantiate(object, position, rotation);
        //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);        // Instantiate shot at shotSpawns position
        if (Input.GetButton("Fire1") & Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //GameObject clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
            //GameObject clone = 
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();
        }
    }

    void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical); // X, Y, Z
        rb.velocity = movement * speed;

        // constrain the ship by setting the value of the rigidbodys position
        rb.position = new Vector3 (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),       // Math functions clamp, clamp the position of players ship inside game area
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);  // Multiply by negative tilt value so it tilts on Z axis when moving left/right on X axis, negative tilt sets correct direction
    }
}
