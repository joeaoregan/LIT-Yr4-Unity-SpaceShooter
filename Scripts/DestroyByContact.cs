/*
 * 20/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * DestroyByContacts.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;                                                                // Explosion GameObject for Asteroid
    public GameObject playerExplosion;                                                          // Player Explosion
    public int scoreValue;                                                                      // Score value for hazards
    //public GameController gameController;                                                     // Instance of class GameController
    private GameController gameController;                                                      // Does not need to be seen in game inspector

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");             // Find the game controller using its tag
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();               // Set the Game Controller
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");                                   // Error to display if GameController script does not load
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            return;                                                                             // Don't destroy the object if it is the Boundary
        }
        Instantiate(explosion, transform.position, transform.rotation);                         // Instantiate an explostion at the tranform point of the GameObject
        if (other.tag == "Player")                                                              // Adds additional explosion only for Player object
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);   // Create Player Explosion
            // Make sure isTrigger is selected on all colliders, or Player Ship pushes Asteroid

            gameController.GameOver();                                                          // End the game when the Player is destroyed
        }
        //GameController.AddScore(scoreValue);                                                  // Send the scorevalue to be added to current score
        gameController.AddScore(scoreValue);                                                    // Can't address the class directly
        Destroy(other.gameObject);                                                              // Destroy gameObject collided with
        Destroy(gameObject);                                                                    // Destroy the gameObject
    }
}
