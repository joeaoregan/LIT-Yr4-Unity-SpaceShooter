/*
 * 22/10/2017
 * Modified by: Joe O'Regan
 *              K00203642
 * 
 * TextInput.cs
 * Text Adventure Unity Tutorial
 * 
 * Allow the player to enter their name, 
 * and stored if score is in high scores
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour {

    public GameObject enableInputText;                              // Show/Hide the name entered
    public GameObject enablePlayer;                                 // The player is hidden while text is entered, and revealed once name is set
    public GameObject enableScore, enableScoreTable;                // Show the score and hide the score table when playing
    public GameObject enableFire, enableMove;                       // Show/Hide the mobile controls depending on if building for mobile platform or not
    public InputField inputField;                                   // Reference to the field for entering the Player name
    GameController controller;                                      // Reference to the game controller

    private void Awake()
    {
        controller = GetComponent<GameController>();                // Get the game controller

        if (PlayerPrefs.GetInt("New Player") == 1)                  // Restart the game, without entering a players name
        {
            EnableDisableUI();                                      // Enable the User Interface
            controller.StartWaves();                                // Start the wave of hazards
        }
        else
        {
            inputField.onEndEdit.AddListener(AcceptStringInput);    // When finished entering text call this function
            inputField.ActivateInputField();                        // Ready to accept keyboard input
        }
    }

    void AcceptStringInput(string userInput)
    {
        controller.StartWaves();                                    // Start spawning waves of enemies
        controller.gameOverText.text = "";                          // Hide the game over message space
        controller.LogStringWithReturn(userInput);                  // If this is not here name is not entered correctly

        EnableDisableUI();                                          // Make UI elements visible after entering name
        InputComplete();                                            // Finished entering text
    }

    void EnableDisableUI()
    {
        enableScoreTable.SetActive(false);                          // Display the high scores table
        enableInputText.SetActive(false);                           // Hide the Input Text field
        enablePlayer.SetActive(true);                               // Show the Player
        enableScore.SetActive(true);                                // Show the score

#if UNITY_ANDROID || UNITY_IPHONE
        enableFire.SetActive(true);                                 // Show fire area
        enableMove.SetActive(true);                                 // Show move area
#endif
    }

    void InputComplete()
    {
        controller.DisplayLoggedText();                             // Display the name entered
        inputField.ActivateInputField();                            // the input field is active
        //inputField.DeactivateInputField();                        // Thought this might hide the Input field, added it as a game object to the game controller instead
        inputField.text = null;                                     // Reset the inputField text
        inputField.enabled = false;                                 // Unable to select the input field to enter text
    }
 }
