﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public GameObject[] groups;
	public GUIText ScoreText;
	public GUIText gameOverMenu;
	public GUIText gameOverScore;

    GameObject nextBlock;
    int[] queue = new int[2];

    int score = 0;
    int rowPoints = 100;

    int lines = 0;

	bool gameOver = false;

	// Use this for initialization
	void Start () {
        SpawnNext();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(lines);

		if (gameOver) {
			gameOverScreen ();
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="multiplier">Bonus for more than one line</param>
    public void UpdateScore(int multiplier)
    {
        score += rowPoints * multiplier;
		ScoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Updates the amount of lines the player has cleared
    /// </summary>
    /// <param name="val">New lines to be added</param>
    public void UpdateLines(int val)
    {
        lines += val;
    }

    /// <summary>
    /// Picks one Tetromino out of the 7 to spawn next
    /// </summary>
    public void SpawnNext()
    {
        int i = Random.Range(0, groups.Length);

        Debug.Log(groups.Length);

        Instantiate(
            //groups[i],
            groups[1],
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
	}
}
