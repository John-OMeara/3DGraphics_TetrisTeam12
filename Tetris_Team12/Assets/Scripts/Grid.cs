using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    // Standard Tetris grid is 10 wide and 22 tall (2 offscreen)
    public static int w = 10;
    public static int h = 22;

    public static Transform[,] grid = new Transform[w, h];

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < w && (int)pos.y >= 0);
    }

    public static void DeleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

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

    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
            DecreaseRow(i);
    }

    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }

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
                c.UpdateScore(multiplier);
            }
        }
        c.UpdateLines(rows);
    }
}
