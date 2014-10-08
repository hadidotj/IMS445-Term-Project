using UnityEngine;
using System.Collections;

public class Main_Menu_Screen : MonoBehaviour {
	public Texture background;
	public int[] scenes;
	void OnGUI(){
		GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height),background);
		if(GUI.Button(new Rect(Screen.width/4-90,Screen.height/2+175,180,65),"Options")){
			Application.LoadLevel(scenes[0]);
		}
		else if(GUI.Button(new Rect(Screen.width/2-90,Screen.height/2+175,180,65),"Play")){
			Application.LoadLevel(scenes[1]);
		}
		else if(GUI.Button(new Rect(Screen.width*3/4-90,Screen.height/2+175,180,65),"Exit")){
			Application.Quit();
		}
	}
}
