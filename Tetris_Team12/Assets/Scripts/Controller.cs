using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
 *  AUTHOR:     John O'Meara
 *              Dylan Murphy
 *
 *  Script acts as a controller for the game. Handles UI, score
 *  and spawning of new game pieces.
 */

public class Controller : MonoBehaviour {

    public GameObject[] groups;     // The Tetrominos

	public GUIText ScoreText;
    public GUIText LinesText;
	public GUIText gameOverMenu;
	public GUIText gameOverScore;

	public GameObject button;
	public string destination;

	public float fallSpeed;     // Value to be passed in from editor and used as fallDelay

    int score = 0;          // Current score
    int rowPoints = 100;    // Base points per row
    int lines = 0;          // Current lines cleared

	bool gameOver = false;

    /// <summary>
    /// Plays sounds
    /// </summary>
	public void playSound()
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}
	
    /// <summary>
    /// On start, spawn the first piece
    /// </summary>
	void Start ()
    {
        SpawnNext();
	}
	
	/// <summary>
    /// If the game ends, go to Game Over screen
    /// </summary>
	void Update ()
    {

		if (gameOver)
        {
			gameOverScreen ();
		}
	}

    /// <summary>
    /// Updates the players score, using multiplier from multiple row clears
    /// </summary>
    /// <param name="multiplier">Bonus for more than one line</param>
    public void UpdateScore(int multiplier)
    {
        score += rowPoints * multiplier;
		ScoreText.text = "SCORE: " + score;
    }

    /// <summary>
    /// Updates the amount of lines the player has cleared
    /// </summary>
    /// <param name="val">New lines to be added</param>
    public void UpdateLines(int val)
    {
        lines += val;
        LinesText.text = "LINES: " + lines;
    }

    /// <summary>
    /// Picks one Tetromino out of the 7 to spawn next
    /// </summary>
    public void SpawnNext()
    {
        int i = Random.Range(0, groups.Length);

        Instantiate(
            groups[i],
            transform.position,
            Quaternion.identity);
    }

	public bool getGameOver()
	{
		return gameOver;
	}

	public void gameOverScreen()
	{
		gameOverMenu.text = "Game Over!";
		gameOverScore.text = "Your Score: " + score;
	}

	public void setGameOver()
	{
		gameOver = true;
		Instantiate (button);
	}

	public float getFallSpeed()
	{
		return fallSpeed;
	}

	public void goMainMenu()
	{
		Application.LoadLevel (destination);
	}
}
