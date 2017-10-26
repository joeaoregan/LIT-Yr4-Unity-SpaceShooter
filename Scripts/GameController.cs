/*
 * 20/09/2017
 * Modified by: Joe O'Regan
 *              K00203642
 * 
 * GameController.cs
 * Space Shooter Unity Tutorial
 * 
 * Main game controller
*/

//using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                                           // For text on canvas
using UnityEngine.SceneManagement;                              // SceneManager

public class GameController : MonoBehaviour {

    //private HighScores hs;                                       // Reference to highscores

    //[HideInInspector]
    public Text displayText;                                    // Show the players name at the top of the screen when entered
    public Text displayFinalScore;                              // Display the final score in the center of the screen above the high scores
    public Text highScoresText;                                 // Display the high scores from file(s)

    List<string> actionLog = new List<string>();

    //public GameObject hazard;
    public GameObject[] hazards;                                // Change to array of hazards
    public Vector3 spawnValues;                                 // Spawn hazards off screen, with a random x value, to spawn anywhere along the width of the screen
    public int hazardCount;                                     // Then number of hazards to create 
    public float spawnWait;                                     // Time to wait before spawning each hazard
    public float startWait;                                     // Time to wait before first wave of hazards begins at beginning of game
    public float waveWait;                                      // Time to wait between waves

    //public GUIText scoreText;                                 // Holds a reference to the GUI Text component
    public Text scoreText;                                      // Score text is now displaye on the UI canvas
    //public GUIText restartText;                               // Replaced with button  
    //public GUIText gameOverText;                              // Game Over Message
    public Text gameOverText;                                   // Game Over message moved to UI canvas
    public GameObject restartButton;                            // Restart button replaces restart text
    public GameObject hideNameText;                             // Hide the name text
    public GameObject enableFinalScoreText;                     // Show/Hide the final score text
    public GameObject hideScore;                                // Show/Hide the score
    public GameObject enableScoreTable;                         // Show/Hide high scores table

    private bool gameOver;                                      // Track when the game is over
    private bool restart;                                       // When it is OK to restart the game
    //public int score;                                         // Holds current score (score is always a whole number)
    private int currentScore;                                   // Does not need to display in game inspector
    private string nameEntered;

    public string getName(){ return nameEntered; }              // Get the players name
    public int getScore() { return currentScore; }              // Get the current score
    public bool isGameOver() { return gameOver; }               // Is the game over or not?

    void Start () {
        gameOver = false;                                       // Game is not over
       // restart = false;
        //restartText.text = "";                                // Replaced with restart button
        restartButton.SetActive(false);                         // Turn restart button off at start of game
        gameOverText.text = "Enter Your Name:";                 // Game over text not displayed at start of game
        currentScore = 0;                                       // Initialise score variable
        nameEntered = "";                                       // Initialise entered name string
        UpdateScore();                                          // Set score to the starting value
        //HighScores();                                         // Show the high scores table
        //hs.CheckHighScores(nameEntered, currentScore);
    }

    public void StartWaves() {
        StartCoroutine(SpawnWaves());                           // Start spawning the waves of hazards
    }

    // Update not required as restart button is now being used
 /*   void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel); // obsolete
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           }
        }
    }*/

    //void SpawnWaves()                                         // Previous SpawnWaves return type was void, this is incompatible with co-routine
    IEnumerator SpawnWaves()                                    // Needed for function to become co-routine
    {
        yield return new WaitForSeconds(startWait);             // A short pause after the game starts to get ready
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z); // Spawn randomly on X coordinate using Random.Range      
                                                                                                                                // Quaternion spawnRotation = new Quaternion();
                Quaternion spawnRotation = Quaternion.identity;                                                                 // Instantiate hazards with no rotation at all
                Instantiate(hazard, spawnPosition, spawnRotation);                                                              // Create the hazard (Asteroid)
                                                                                                                                // WaitForSeconds(spawnWait);
                yield return new WaitForSeconds(spawnWait);                                                                     // needed for co-routine
            }
            yield return new WaitForSeconds(waveWait);                                                                          // Time between waves

            if (gameOver)
            {
                //restartText.text = "Press 'R' for Restart";               // Restart the game by pressing R --> Replaced with restart button
                restartButton.SetActive(true);                              // Show the restart button
                //restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        if(!gameOver)
        currentScore += newScoreValue;                                      // Update the score value
        UpdateScore();                                                      // Update the score text to current score
    }

    // Update the score
    void UpdateScore()
    {
        scoreText.text = "Score: " + currentScore;                          // Displays the score, score is updated in start, and when a point is scored
    }

    public void GameOver()
    {
        //string overMessage = "Game Over " + displayText.text + "!";       // Causes game to keep playing instead of ending????
        gameOverText.text = "Game Over!";                                   // Update the game over text

        displayFinalScore.text = "Score For " + getName() + getScore();

        gameOver = true;                                                    // The game is over

        //HighScores();                                                       // Check if scores is in high scores, and display new high scores

        nameEntered = System.Text.RegularExpressions.Regex.Replace(nameEntered, @"\t|\n|\r", ""); // remove new line character
        //hs.CheckHighScores(nameEntered, currentScore);

        enableScoreTable.SetActive(true);                                   // Display the high scores table
        enableFinalScoreText.SetActive(true);                               // Display the final score
        hideNameText.SetActive(false);                                      // Hide the Player name at top of screen
        hideScore.SetActive(false);                                         // Hide the score at top of screen
    }

    public void RestartGame()
    {
        //Application.LoadLevel(Application.loadedLevel);                   // Restart the current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HighScores() {      
        int[] scores = new int[11];                                                                     // Array of high scores
        string[] names = new string[11];                                                                // Array of names for high scores
                                                                                                        /*
                                                                                                        // Read and show each line from the file.
                                                                                                        string lineScore = "";
                                                                                                        int i = 0;
                                                                                                        string lineName = "";

                                                                                                        // Read scores and names from files
                                                                                                        using (StreamReader sr = new StreamReader("scores.txt")) {                                      // Read scores from scores.txt
                                                                                                            using (StreamReader sr2 = new StreamReader("names.txt")) {                                  // Read names from names.txt

                                                                                                                highScoresText.text = "High Scores:\n";   // reset the scores table

                                                                                                                while (((lineName = sr2.ReadLine()) != null) && (lineScore = sr.ReadLine()) != null)    // While there are lines of text to read
                                                                                                                {
                                                                                                                    int parseScore = System.Int32.Parse(lineScore);                                     // Parse the string to an int
                                                                                                                    if (parseScore == 0) continue;                                                      // No need to display 0 value score

                                                                                                                    scores[i] = parseScore;                                                             // Add the score to the scores array
                                                                                                                    names[i++] = lineName;                                                              // Add name to names array, and increment i by 1

                                                                                                                    Debug.Log(lineName + "  " + lineScore + "\n");
                                                                                                                    //highScoresText.text += i + ". " + lineScore + " " + lineName + "\n";
                                                                                                                    highScoresText.text += i + ". " + lineName + " " + lineScore + "\n";                // will be one, i has already incremented
                                                                                                                }

                                                                                                                Debug.Log("Name and Scores Arrays:\n");
                                                                                                                for (int j = 0; j < 10; j++)
                                                                                                                {
                                                                                                                    if (scores[j] == 0) continue;                                                       // skip 0 values for score
                                                                                                                    Debug.Log(names[j] + " " + scores[j] + "\n");                                       // Display the names and scores in Unity console
                                                                                                                }

                                                                                                                int n = 11, tempScore = 0;
                                                                                                                string tempName = "";

                                                                                                                scores[10] = currentScore;                                                              // Set the 11th score to the current score
                                                                                                                names[10] = System.Text.RegularExpressions.Regex.Replace(nameEntered, @"\t|\n|\r", ""); // remove new line character

                                                                                                                Debug.Log("\nAdded current name and score:\n");
                                                                                                                for (int s = 0; s < 11; s++)
                                                                                                                {
                                                                                                                    Debug.Log(s + ". " + names[s] + " " + scores[s] + "\n");                            // Check the current name and score is added
                                                                                                                }

                                                                                                                Debug.Log("\nCurrent Name and Score: " + nameEntered + currentScore);

                                                                                                                // Swap the scores
                                                                                                                for (int x = 1; x < n; x++)                                                             // Check all the scores
                                                                                                                {
                                                                                                                    for (int y = 0; y < n - x; y++)                                                     // Against every other score
                                                                                                                    {
                                                                                                                        if (scores[y] < scores[y + 1])                                                  // Sorting the largest score first
                                                                                                                        {
                                                                                                                            tempScore = scores[y];                                                      // Do swapping
                                                                                                                            tempName = names[y];

                                                                                                                            scores[y] = scores[y + 1];
                                                                                                                            names[y] = names[y + 1];

                                                                                                                            scores[y + 1] = tempScore;
                                                                                                                            names[y + 1] = tempName;
                                                                                                                        }
                                                                                                                    }
                                                                                                                }

                                                                                                                Debug.Log("\nSorted High Scores:\n");
                                                                                                                for (int s = 0; s < 11; s++)                                                            // Check the scores are sorted correctly
                                                                                                                {
                                                                                                                    if (scores[s] == 0) continue;                                                       // skip 0 values for score
                                                                                                                    Debug.Log(s + ". " + names[s] + " " + scores[s] + "\n");                            // Display the name and score
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        */

        DisplayHighScores(scores, names);                                                                   // Read and display the high scores table

        // Write the names and score to separate files (Was the simplest solution)
        using (StreamWriter sw = new StreamWriter("scores.txt")) {
            //foreach (int s in scores)
            for (int s = 0; s < 10;s++)                                                                     // Make sure only 10 scores are output to file
            {
                sw.WriteLine(scores[s]);
            }
        }
        using (StreamWriter sw = new StreamWriter("names.txt")) {
            foreach (string s in names) {                                                                   // There wasn't any need to change this one
                sw.WriteLine(s);
            }
        }

    }

    void DisplayHighScores(int[] arrScores, string[] arrNames) {
        int i = 0;
        string lineName = "", lineScore = "";
        int n = 11, tempScore = 0;
        string tempName = "";

        // Read the names and scores from file, and to array
        using (StreamReader sr = new StreamReader("scores.txt"))                                            // Read the scores from the scores.txt file
        {
            using (StreamReader sr2 = new StreamReader("names.txt"))                                        // Read the names from the names.txt file
            {
                while (((lineName = sr2.ReadLine()) != null) && (lineScore = sr.ReadLine()) != null)        // While there are lines to read in the files
                {
                    int parseScore = System.Int32.Parse(lineScore);                                         // parse the score from each line in scores.txt
                    if (parseScore == 0) continue;                                                          // if the score is 0 skip the rest
                    
                    arrScores[i] = parseScore;                                                              // Add the score to the scores array
                    arrNames[i++] = lineName;                                                               // add name to names array, and increment i                    
                }                

                // Add the current name and score to position 11 of the high scores list
                arrScores[10] = currentScore;                                                               // Set the 11th score to the current score
                arrNames[10] = System.Text.RegularExpressions.Regex.Replace(nameEntered, @"\t|\n|\r", "");  // remove new line character

                if (currentScore > arrScores[0])
                    highScoresText.text = "New High Score:\n";                                              // Reset the scores table beginning with new high score message
                else
                    highScoresText.text = "High Scores:\n";                                                 // Reset the scores table

                // Swap the scores
                for (int x = 1; x < n; x++)                                                                 // Check all the scores
                {
                    for (int y = 0; y < n - x; y++)                                                         // Against every other score
                    {
                        if (arrScores[y] < arrScores[y + 1])                                                // Sorting the largest score first
                        {
                            tempScore = arrScores[y];                                                       // Do swapping
                            tempName = arrNames[y];

                            arrScores[y] = arrScores[y + 1];
                            arrNames[y] = arrNames[y + 1];

                            arrScores[y + 1] = tempScore;
                            arrNames[y + 1] = tempName;
                        }
                    }
                }

                // Display updated scores
                for (int z = 0; z < 10; z++)
                {
                    highScoresText.text += (z+1) + ". " + arrNames[z] + " " + arrScores[z] + "\n";          // 1st line Will be one, i has already incremented, showing name and score
                }
            }
        }
    }

    
    // From text adventure tutorial
    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
    }
    
    public void DisplayLoggedText()
    {
        // string logAsText = string.Join("\n", actionLog.ToArray());
        nameEntered = string.Join("\n", actionLog.ToArray());
        displayText.text = nameEntered;                                                                     // Set the name at the top of the screen to the name entered

        gameOverText.text = "";                                                                             // Update the game over text
    }
}
