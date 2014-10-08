﻿using UnityEngine;
using System.Collections;

public class Readying_Menu_Screen : MonoBehaviour {

	public Texture background;
	private bool ready = false;

	void OnGUI(){
		GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height),background);
		if(!ready && GUI.Button(new Rect(Screen.width/2-90,Screen.height/2+175,180,65),"Return")){
			Application.LoadLevel(1);
		}
	}


	/*
	 * There will be more code to this script. Currently the script iss to show proof of concept of 
	 * moving between screens as a menu system.
	 */
}
