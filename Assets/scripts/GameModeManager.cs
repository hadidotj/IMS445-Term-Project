using UnityEngine;
using System.Collections;

public class GameModeManager : MonoBehaviour {
	
	private int gameMode;
	// Use this for initialization
	void Start () {
		gameMode = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameMode == 0) {
			// Play Search and Destroy
		} else if(gameMode == 1) {
			// Play Team Death Match
		} else {
			// Play Free For All
		}

	}
}
