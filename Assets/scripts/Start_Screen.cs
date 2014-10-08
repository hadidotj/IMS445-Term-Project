using UnityEngine;
using System.Collections;

public class Start_Screen : MonoBehaviour {

	public Texture background;
	
	void OnGUI(){
		GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height),background);
		if(GUI.Button(new Rect(Screen.width/2-90,Screen.height/2+175,180,65),"Start the Game")){
			Application.LoadLevel(1);
		}
	}
}
