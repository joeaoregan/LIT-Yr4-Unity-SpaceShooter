/*
 * 20/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * Mover.cs
*/

//using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    //private Rigidbody rb;
   // public float speed;             // Set the Bolt speed
   /*
    // Use this for initialization
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = transform.forward * speed;

        //Rigidbody rb = (Rigidbody)GetComponent(typeof(Rigidbody));
        //Vector3 movement = new Vector3(0.0f, 0.0f, 1.0f);
        //GetComponent<Rigidbody>().velocity = movement * speed;

        Rigidbody rb = (Rigidbody)GetComponent(typeof(Rigidbody));
        rb.velocity = transform.forward * speed;
    }
    */
    public float speed;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}
