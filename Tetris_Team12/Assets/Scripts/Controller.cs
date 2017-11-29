using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public GameObject[] groups;

    int score = 0;
    int rowPoints = 100;

	// Use this for initialization
	void Start () {
        SpawnNext();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(score);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="multiplier">Bonus for more than one line</param>
    public void UpdateScore(int multiplier)
    {
        score += rowPoints * multiplier;
    }

    /// <summary>
    /// Picks one Tetromino out of the 7 to spawn next
    /// </summary>
    public void SpawnNext()
    {
        int i = Random.Range(0, groups.Length);

        Debug.Log(groups.Length);

        Instantiate(groups[i],
            transform.position,
            Quaternion.identity);
    }
}
