/*
 * 20/09/2017
 * Modified by: Joe O'Regan
 *              K00203642
 * 
 * PlayerController.cs
 * Space Shooter Unity Tutorial
 * 
 * Handle player movement and firing
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boundary class, does not inherit, and is serialised (needs to be serialised to view in inspector)
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;                                            // Clamp our position between the values set for min and max on x and y axes
}

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    public float speed;                                                             // Control the speed. Allow the ship to move more than 1 unit per second
    public float tilt;                                                              // Add bank/tilt to the player ship, so it rotates on x axis
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;                                                          // Time gap between firing bullets
                                     
    public SimpleTouchPad touchpad;
    public SimpleTouchAreaButton areaButton;                                        // Create area button

    private float nextFire;
    private Quaternion calibrationQuaternion;                                       // Quaternion value for mobile devide accelermeter input

    private AudioSource audioSource;                                                // Add Player Weapon sound effect

    private void Start()
    {
        rb = GetComponent<Rigidbody>();                                             // Unity 5 can no longer access components using shorthand helper references
        audioSource = GetComponent<AudioSource>();                                  // Same for Audio

        CallibrateAccelerometer();                                                  // Set the accelerometer starting position for mobile device
    }

    private void Update()
    {
        // Alter controls for different devices
        if (SystemInfo.deviceType == DeviceType.Handheld)                           // For mobile
        {
            if (areaButton.CanFire() & Time.time > nextFire)                        // If the Fire Zone is pressed, and time to fire is true, can fire shot
            {
                nextFire = Time.time + fireRate;
                //GameObject clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
                //GameObject clone = 
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                audioSource.Play();
            }
        }
        else                                                                        // All other devices
        {
            if (Input.GetButton("Fire1") & Time.time > nextFire)
            {                 // Replaced with Firezone
                nextFire = Time.time + fireRate;
                //Instantiate(object, position, rotation);
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);              // Instantiate shot at shotSpawns position
                audioSource.Play();
            }
        }
    }

    void FixedUpdate() {
        Vector3 movement;

        if (SystemInfo.deviceType == DeviceType.Handheld)                           // For mobile device
        {
            // Vector3 acceleration = Input.acceleration;                           // Ask input class to look at current device and get acceleration
            Vector3 accelerationRaw = Input.acceleration;                           // Ask input class to look at current device and get acceleration
            //Vector3 acceleration = FixedAcceleration(accelerationRaw);              // Fix the acceleration
            //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);   // X, Y, Z Keyboard controls
                                    
            Vector2 direction = touchpad.GetDirection();
            //Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);       // Grab direction from touchpad
            movement = new Vector3(direction.x, 0.0f, direction.y);                 // Grab direction from touchpad
        }
        else                                                                        // All other builds
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);   // X, Y, Z Keyboard controls
            movement = new Vector3(moveHorizontal, 0.0f, moveVertical);             // X, Y, Z Keyboard controls
        }

        rb.velocity = movement * speed;

        // constrain the ship by setting the value of the rigidbodys position
        rb.position = new Vector3 (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),               // Math functions clamp, clamp the position of players ship inside game area
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);          // Multiply by negative tilt value so it tilts on Z axis when moving left/right on X axis, negative tilt sets correct direction
    }

    // Used to calibrate the Input.acceleration input
    void CallibrateAccelerometer()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    }

    // Get the 'calibrated' value from the input
    Vector3 FixedAcceleration (Vector3 acceleration)
    {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }
}
