/*
 * 20/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * GameController.cs
*/

//using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                                           // For text on canvas
using UnityEngine.SceneManagement;                              // SceneManager

public class GameController : MonoBehaviour {

    //[HideInInspector]
    public Text displayText;
    public Text displayFinalScore;
    public Text highScoresText;

    List<string> actionLog = new List<string>();

    //public GameObject hazard;
    public GameObject[] hazards;                                // Change to array of hazards
    public Vector3 spawnValues;
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
   // public bool getRestart() { return restart; }
    //public int score;                                         // Holds current score (score is always a whole number)
    private int currentScore;                                   // Does not need to display in game inspector
    //private bool nameEntered;
    private string nameEntered; 

    void Start () {
        //nameEntered = false;
        gameOver = false;
       // restart = false;
        //restartText.text = "";                                // Replaced with restart button
        restartButton.SetActive(false);                         // Turn restart button off at start of game
        //gameOverText.text = "";                               // Game over text not displayed at start of game
        gameOverText.text = "Enter Your Name:";                 // Game over text not displayed at start of game
        currentScore = 0;                                       // Initialise score variable
        nameEntered = "";                                       // Initialise entered name string
        UpdateScore();                                          // Set score to the starting value
        //SpawnWaves();
        //if(nameEntered)
        //StartCoroutine(SpawnWaves());                         // Need to explictaly call coroutine
        //StartWaves();
        HighScores();
     }

    public void StartWaves() {
        StartCoroutine(SpawnWaves());
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
                restartButton.SetActive(true);
                //restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
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
        //gameOverText.text = "Game Over " _+ hideNameText.guiText + "!";   // Update the game over text
        //gameOverText.text = "Game Over " + displayText.text + "!";        // Update the game over text
        //string overMessage = "Game Over " + displayText.text + "!";       // Causes game to keep playing instead of ending????
        gameOverText.text = "Game Over!";                                   // Update the game over text
        //gameOverText.text = overMessage;                                  // Update the game over text

        displayFinalScore.text = "Score For " + nameEntered + currentScore;

        gameOver = true;                                                    // The game is over
        HighScores();                                                       // Check if scores is in high scores, and display new high scores

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
        /*
        //Debug.Log("Scores Test");
        highScoresText.text = "High Scores:\n";                             // reset the scores table

        FileStream F = new FileStream("HighScores.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        //string[] swapText = new string[11];                               // Array of 10 scores + 1 for swapping

        // Read the Scores
        for (int i = 0; i < 10; i++)
        {
            highScoresText.text += F.ReadByte() + "\n";
        }

        for (int i = 1; i <= 10; i++)
        {
            F.WriteByte((byte)i);
        }

        F.Position = 0;

        for (int i = 0; i < 10; i++)
        {
            //Console.Write(F.ReadByte() + " ");
            //Debug.Log(F.ReadByte() + " ");
            highScoresText.text += F.ReadByte() + "\n";
        }

        F.Close();  // Close the file
                    //Console.ReadKey();

        */
        
        /*
        // READ TEXT FILE

        // Create an instance of StreamReader to read from a file.
        // The using statement also closes the StreamReader.
        //using (StreamReader sr = new StreamReader("c:/test.txt"))
        using (StreamReader sr = new StreamReader("test.txt"))
        {
            string line;

            // Read and display lines from the file until 
            // the end of the file is reached. 
            while ((line = sr.ReadLine()) != null)
            {
                Debug.Log(line);
            }
        }
        */

        // READ / WRITE TEXT FILE

        //int[] scores = new int[11];
        //     string[] name = new string[11];

        /*
        string[] scoreString = new string[] { "20 player 1", "10 player 2" };

        using (StreamWriter sw = new StreamWriter("scoresAndNames.txt"))
        {
            foreach (string s in scoreString)
            {
                sw.WriteLine(s);
            }
        }
        */
        /*
             // Read and show each line from the file.
             string lineScore = "";
             int i = 0;
             using (StreamReader sr = new StreamReader("scores.txt"))
             {
                 while ((lineScore = sr.ReadLine()) != null)
                 {
                     scores[i++] = System.Int32.Parse(lineScore);
                     //Console.WriteLine(lineScore);
                     Debug.Log(i + scores[i] + "\n");

                 }
             }

             string lineName = "";
             using (StreamReader sr = new StreamReader("names.txt"))
             {
                 while ((lineName = sr.ReadLine()) != null)
                 {
                     //Console.WriteLine(line);
                     Debug.Log(lineName);
                 }
             } */

        // Read and show each line from the file.
        string lineScore = "";
        int i = 0;
        string lineName = "";

        int[] scores = new int[11];
        string[] names = new string[11];

        // Read scores and names from files
        using (StreamReader sr = new StreamReader("scores.txt")) {
            using (StreamReader sr2 = new StreamReader("names.txt")) {
                /*
                while ((lineName = sr2.ReadLine()) != null || (lineScore = sr.ReadLine()) != null) {
                    scores[i++] = System.Int32.Parse(lineScore);
                    Debug.Log(lineName);
                    Debug.Log(i + scores[i] + "\n");
                }*/

                highScoresText.text = "High Scores:\n";   // reset the scores table

                while (((lineName = sr2.ReadLine()) != null) && (lineScore = sr.ReadLine()) != null)
                {
                    //scores[i] = System.Int32.Parse(lineScore);      // dont increment i here dumbass
                    int parseScore = System.Int32.Parse(lineScore);
                    if (parseScore == 0) continue;

                    scores[i] = parseScore;
                    names[i++] = lineName;                          // add name and increment i

                    Debug.Log(lineName + "  " + lineScore + "\n");
                    highScoresText.text += i + ". " + lineScore + " " + lineName + "\n";    // will be one, i has already incremented
                }

                Debug.Log("Name and Scores Arrays:\n");
                for (int j = 0; j < 10; j++)
                {
                    // if (scores[j] == 0) continue;                       // skip 0 values for score
                    Debug.Log(names[j] + " " + scores[j] + "\n");
                }

                int n = 11, tempScore = 0;
                string tempName = "";

                scores[10] = currentScore;  // Set the 11th score to the current score
                //names[10] = nameEntered;    // Set the 11th name to the name entered
                //string replacement = System.Text.RegularExpressions.Regex.Replace(names[10], @"\t|\n|\r", "");
                names[10] = System.Text.RegularExpressions.Regex.Replace(nameEntered, @"\t|\n|\r", "");  // remove new line character
                Debug.Log("\nCurrent name in names array:" + names[10] + "\n");
                Debug.Log("\nAdded current name and score:\n");

                for (int s = 0; s < 11; s++)
                {
                    Debug.Log(s + ". " + names[s] + " " + scores[s] + "\n");
                }

                Debug.Log("Current Score: " + currentScore + "\n");
                string nameAndScore = " " + currentScore + "\n";
                Debug.Log("\nCurrent Name and Score: " + nameEntered + nameAndScore);

                // Swap the scores
                for (int x = 1; x < n; x++)
                {
                    for (int y = 0; y < n - x; y++)
                    {
                        if (scores[y] < scores[y + 1]) // sorts largest first
                        {
                            tempScore = scores[y];
                            tempName = names[y];

                            scores[y] = scores[y + 1];
                            names[y] = names[y + 1];

                            scores[y + 1] = tempScore;
                            names[y + 1] = tempName;
                        }
                    }
                }

                Debug.Log("\nSorted High Scores:\n");

                for (int s = 0; s < 11; s++)
                {
                    //if (scores[j] == 0) continue;                       // skip 0 values for score
                    Debug.Log(s + ". " + names[s] + " " + scores[s] + "\n");
                }
                /*
                while ((lineScore = sr.ReadLine()) != null) {
                    scores[i++] = System.Int32.Parse(lineScore);
                    //Console.WriteLine(lineScore);
                    Debug.Log(i + scores[i] + "\n");
                } */
            }
        }
        
        // write to file
        using (StreamWriter sw = new StreamWriter("scores.txt")) {
            //foreach (int s in scores)
            for (int s = 0; s < 10;s++) // Make sure only 10 scores are output to file
            {
                sw.WriteLine(scores[s]);
            }
        }
        using (StreamWriter sw = new StreamWriter("names.txt")) {
            foreach (string s in names) {   // There wasn't any need to change this one
                sw.WriteLine(s);
            }
        }
        /*
        i = 0;
        using (StreamReader sr = new StreamReader("scores.txt")) {
            using (StreamReader sr2 = new StreamReader("names.txt")) {
                highScoresText.text = "High Scores:\n";   // reset the scores table
                while (((lineName = sr2.ReadLine()) != null) && (lineScore = sr.ReadLine()) != null) {
                    //scores[i] = System.Int32.Parse(lineScore);      // dont increment i here dumbass
                    int parseScore = System.Int32.Parse(lineScore);
                    if (parseScore == 0) continue;

                    scores[i] = parseScore;
                    names[i++] = lineName;                          // add name and increment i

                    //Debug.Log(lineName + "  " + lineScore + "\n");
                    highScoresText.text += i + ". " + lineScore + " " + lineName + "\n";    // will be one, i has already incremented
                }
            }
        }
        */
        DisplayHighScores(scores, names);
    }

    void DisplayHighScores(int[] arrScore, string[] arrString) {
        int i = 0;
        string lineName = "", lineScore = "";

        using (StreamReader sr = new StreamReader("scores.txt"))
        {
            using (StreamReader sr2 = new StreamReader("names.txt"))
            {
                highScoresText.text = "High Scores:\n";   // reset the scores table
                while (((lineName = sr2.ReadLine()) != null) && (lineScore = sr.ReadLine()) != null)
                {
                    int parseScore = System.Int32.Parse(lineScore);
                    if (parseScore == 0) continue;

                    //scores[i] = parseScore;
                    //names[i++] = lineName;
                    arrScore[i] = parseScore;
                    arrString[i++] = lineName;                                                          // add name and increment i

                    //Debug.Log(lineName + "  " + lineScore + "\n");
                    highScoresText.text += i + ". " + lineScore + " " + lineName + "\n";                // will be one, i has already incremented
                }
            }
        }
    }


    // From text adventure tutorial
    public void LogStringWithReturn(string stringToAdd)
    {
        //nameEntered = true;
        //StartCoroutine(SpawnWaves());

        actionLog.Add(stringToAdd + "\n");
        //gameOverText.text = "";                                   // Update the game over text
        //nameEntered = true;
    }

    public void DisplayLoggedText()
    {
       // hideNameText.SetActive(true);
        //displayText.text = logAsText;

        // string logAsText = string.Join("\n", actionLog.ToArray());
        nameEntered = string.Join("\n", actionLog.ToArray());
        //displayText.text = "Player: " + logAsText;
        displayText.text = nameEntered;
        //displayText.text = "test";

        gameOverText.text = "";                                     // Update the game over text
    }
    /*
    public void GetPlayerName() {
        gameOverText.text = "Enter Name:";                          // Update the game over text
    }

    public void FinishedName()
    {
        gameOverText.text = "";                                     // Update the game over text
    }

    public void nameDone() {
        //nameEntered = true;
        StartCoroutine(SpawnWaves());
    }
    */
}
