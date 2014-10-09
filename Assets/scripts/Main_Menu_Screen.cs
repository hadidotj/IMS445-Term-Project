using UnityEngine;
using System.Collections;

public class Main_Menu_Screen : MonoBehaviour {
	public Texture background;
	void OnGUI(){
		GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height),background);
		if(GUI.Button(new Rect(Screen.width/4-90,Screen.height/2+175,180,65),"Options")){
			Application.LoadLevel("options_menu");
		}
		else if(GUI.Button(new Rect(Screen.width/2-90,Screen.height/2+175,180,65),"Play")){
			Application.LoadLevel("getting_ready_to_play");
		}
		else if(GUI.Button(new Rect(Screen.width*3/4-90,Screen.height/2+175,180,65),"Exit")){
			Application.Quit();
		}
	}
}
