using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour {

	public string destination;

	public void LoadScene () {
		Application.LoadLevel (destination);
	}

	public void goQuit()
	{
		Application.Quit();
	}
}
