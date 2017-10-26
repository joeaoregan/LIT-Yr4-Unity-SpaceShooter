using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour {

    public GameObject enableInputText;
    public GameObject enablePlayer;
    //public GameObject enablePlayerName;
    public GameObject enableScore, enableScoreTable;
    public GameObject enableFire, enableMove;
    public InputField inputField;
    //public GameObject inputField;
    GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
        //controller.gameOverText.text = "Enter Your Name:";        // Update the game over text
        inputField.onEndEdit.AddListener(AcceptStringInput);  
        // controller.StartWaves();
        inputField.ActivateInputField();                            // Ready to accept keyboard input
    }

    void AcceptStringInput(string userInput)
    {
        controller.StartWaves();                                    // Start spawning waves of enemies
        controller.gameOverText.text = "";                          // Hide the game over message space
        //userInput = userInput.ToLower();                          // Convert user input to lower case
        controller.LogStringWithReturn(userInput);

        EnableDisableUI();                                          // Make UI elements visible after entering name
        InputComplete();                                            // Finished entering text
    }

    void EnableDisableUI()
    {
        enableScoreTable.SetActive(false);                          // Display the high scores table
        enableInputText.SetActive(false);                           // Hide the Input Text field
        enablePlayer.SetActive(true);                               // Show the Player
        //enablePlayerName.SetActive(true);                         // Show the Player Name
        enableScore.SetActive(true);                                // Show the score

#if UNITY_ANDROID || UNITY_IPHONE
        enableFire.SetActive(true);                                 // Show fire area
        enableMove.SetActive(true);                                 // Show move area
#endif
    }

    void InputComplete()
    {
        controller.DisplayLoggedText();
        inputField.ActivateInputField();
        //inputField.DeactivateInputField();
        inputField.text = null;
        inputField.enabled = false;
        //inputField.SetActive(false);
        //DestroyImmediate(inputField);
        ///inputField.DeactivateInputField();
        //Destroy(inputField);    // doesn't work
        //inputField.SetActive(false);
    }
 }
