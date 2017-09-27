/*
 * 20/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * GameController.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;                              // SceneManager

public class GameController : MonoBehaviour {

    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;                                     // Then number of hazards to create 
    public float spawnWait;                                     // Time to wait before spawning each hazard
    public float startWait;                                     // Time to wait before first wave of hazards begins at beginning of game
    public float waveWait;                                      // Time to wait between waves

    public GUIText scoreText;                                   // Holds a reference to the GUI Text component
    public GUIText restartText;                                 
    public GUIText gameOverText;

    private bool gameOver;                                      // Track when the game is over
    private bool restart;                                       // When it is OK to restart the game
    //public int score;                                         // Holds current score (score is always a whole number)
    private int score;                                          // Does not need to display in game inspector

    void Start () {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;                                              // Initialise score variable
        UpdateScore();                                          // Set score to the starting value
        //SpawnWaves();
        StartCoroutine(SpawnWaves());                           // Need to explictaly call coroutine
	}

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel); // obsolete
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    //void SpawnWaves()                                         // Previous SpawnWaves return type was void, this is incompatible with co-routine
    IEnumerator SpawnWaves()                                    // Needed for function to become co-routine
    {
        yield return new WaitForSeconds(startWait);             // A short pause after the game starts to get ready
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
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
                restartText.text = "Press 'R' for Restart";     // Restart the game by pressing R
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;                                 // Update the score value
        UpdateScore();                                          // Update the score text to current score
    }

    // Update the score
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;                     // Displays the score, score is updated in start, and when a point is scored
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
