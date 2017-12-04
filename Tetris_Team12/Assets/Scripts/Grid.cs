using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
 *  AUTHOR:     John O'Meara,
 *              Dylan Murphy,
 *              John Codd
 *              
 *  Script that controls the game field. Handles boundary checking, 
 *  block clearing and the bomb explosion logic.
 */

public class Grid : MonoBehaviour {

    // Standard Tetris grid is 10 wide and 22 tall (2 offscreen)
    public static int w = 10;
    public static int h = 22;

    public static Transform[,] grid = new Transform[w, h];

    /// <summary>
    /// Rounds vector to closest integer values (to fit in grid)
    /// </summary>
    /// <param name="v">Vector to round</param>
    /// <returns>Rounded value</returns>
    public static Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    /// <summary>
    /// Checks if the given position is within the grid
    /// </summary>
    /// <param name="pos">Position to check</param>
    /// <returns>Whether it's inside</returns>
    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < w && (int)pos.y >= 0);
    }

    /// <summary>
    /// Removes row from game field
    /// </summary>
    /// <param name="y">Row to be removed</param>
    public static void DeleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
		{
			Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    /// <summary>
    /// Bomb clears all 8 adjacent blocks
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static void BombExplode(int x, int y)
    {
        if (grid[x, y + 1] != null)
        {
            Destroy(grid[x, y + 1].gameObject);
        }
        if (x != 0)
        {
            if (grid[x - 1, y + 1] != null)
            {
                Destroy(grid[x - 1, y + 1].gameObject);
            }
            if (grid[x - 1, y] != null)
            {
                Destroy(grid[x - 1, y].gameObject);
            }
        }
        if (x != 9)
        {
            if (grid[x + 1, y + 1] != null)
            {
                Destroy(grid[x + 1, y + 1].gameObject);
            }
            if (grid[x + 1, y] != null)
            {
                Destroy(grid[x + 1, y].gameObject);
            }
        }
        if (x != 0 && y != 0)
        {
            if (grid[x - 1, y - 1] != null)
            {
                Destroy(grid[x - 1, y - 1].gameObject);
            }
        }
        if (y != 0)
        {
            if (grid[x, y - 1] != null)
            {
                Destroy(grid[x, y - 1].gameObject);
            }
        }
        if (x != 9 && y != 0)
        {
            if (grid[x + 1, y - 1] != null)
            {
                Destroy(grid[x + 1, y - 1].gameObject);
            }
        }
    }

    /// <summary>
    /// Moves a row down by one
    /// </summary>
    /// <param name="y">The row to be moved down</param>
    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    /// <summary>
    /// Moves down all rows above a given row by one.
    /// </summary>
    /// <param name="y">The row</param>
    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
        {
            DecreaseRow(i);
        }
    }

    /// <summary>
    /// Checks if a row is full
    /// </summary>
    /// <param name="y">Row to be checked</param>
    /// <returns></returns>
    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Calls delete functions for all full rows.
    /// Keeps track of score and lines, updates these values.
    /// </summary>
    public static void DeleteFullRows()
    {
        Controller c = FindObjectOfType<Controller>();
        int multiplier = 0;
        int rows = 0;
        for (int y = 0; y < h; ++y)
        {
            if (IsRowFull(y))
            {
                multiplier++;
                rows++;
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
				FindObjectOfType<Controller> ().playSound();
				FindObjectOfType<Controller> ().GetComponent<ParticleSystem>().Play ();
				c.UpdateScore(multiplier);
            }
        }
        c.UpdateLines(rows);
    }
}
