using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
 *  AUTHOR:     John O'Meara,
 *              Dylan Murphy
 *              
 *  Script that controls the game field. Handles boundary checking, 
 *  block clearing and the bomb explosion logic.
 */

public class Group : MonoBehaviour {

    float lastFall = 0;         // Time that the last auto fall was triggered
    float fallDelay = 0.25f;    // The time between auto falls

    bool fall = false;          // If it immediatly drops or not

	AudioSource audio;

    /// <summary>
    /// Called when the Tetromino is created. 
    /// 
    /// Sets up the audio and fallDelay variable. Checks if the starting position
    /// is still valid, else it's a game over.
    /// </summary>
    void Start()
    {
		audio = GetComponent<AudioSource>();
		fallDelay = FindObjectOfType<Controller> ().getFallSpeed ();

        if (!IsValidGridPos())
        {
            Destroy(gameObject);
			FindObjectOfType<Controller> ().setGameOver ();
        }
    }

    /// <summary>
    /// Called every update frame.
    /// 
    /// Checks player input and moves the block downwards.
    /// </summary>
    void Update()
    {
        // LEFT
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(new Vector3(-1, 0, 0));
        }

        // RIGHT
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(new Vector3(1, 0, 0));
        }

        // ROTATE
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            
            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
        }

        // DOWN
        else if (Input.GetKeyDown(KeyCode.DownArrow)    // Down key is pressed
                || Time.time - lastFall >= fallDelay    // Automatic fall interval
                || fall)                                // Space key, immediate fall
        {
            transform.position += new Vector3(0, -1, 0);
            
            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
				transform.position += new Vector3(0, 1, 0);
                
                Grid.DeleteFullRows();

				if (FindObjectOfType<Controller> ().getGameOver() == false)
                {
					FindObjectOfType<Controller> ().SpawnNext ();
					audio.Play();
					gameObject.GetComponentInParent<ParticleSystem> ().Stop();
				}

                enabled = false;
            }

            lastFall = Time.time;
        }

        // SPACE, immediate fall
        if(Input.GetKeyDown(KeyCode.Space))
        {
            fall = true;
        }
    }

    /// <summary>
    /// Attempts to move the tetromino a certain direction. If it's not valid it
    /// reverts the move.
    /// </summary>
    /// <param name="dir">Direction to move towards</param>
    void Move(Vector3 dir)
    {
        transform.position += dir;
        
        if (IsValidGridPos())
        {
            UpdateGrid();
        }
        else
        {
            transform.position += dir *-1;
        }
    } 

    /// <summary>
    /// Checks if the new position is valid.
    /// </summary>
    /// <returns>True if valid, false if invalid</returns>
    bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);
            Debug.Log(v);
        
            if (!Grid.InsideBorder(v))
            {
                return false;
            }
            
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Updates the representation of the playfield
    /// </summary>
    void UpdateGrid()
    {
        for (int y = 0; y < Grid.h; ++y)
        {
            for (int x = 0; x < Grid.w; ++x)
            {
                if (Grid.grid[x, y] != null)
                {
                    if (Grid.grid[x, y].parent == transform)
                    {
                        Grid.grid[x, y] = null;
                    }
                }
            }
        }
        
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }
}
