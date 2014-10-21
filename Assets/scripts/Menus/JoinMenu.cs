using UnityEngine;
using System.Collections;

public class JoinMenu : AbstractMenu {

	public Texture2D header;
	public Texture2D back;
	
	private int currentHoveredOver = -1;
	
	public override void Draw(){
		// Draw header
		GUI.DrawTexture(new Rect(Screen.width/2.0f - header.width/2.0f, 10, header.width, header.height), header);
		
		// Back Button
		GUI.backgroundColor = Color.red;
		Rect backButtonRect = new Rect(10.0f, Screen.height-back.height-10.0f, back.width, back.height);
		Vector3 mouse = Input.mousePosition;
		mouse.y = Screen.height-mouse.y;
		if(GUI.Button (backButtonRect, back)){
			ButtonPressed("MainMenu");
		} else if(currentHoveredOver != 0 && backButtonRect.Contains(mouse)) {
			currentHoveredOver = 0;
			ButtonHover();
		} else if(currentHoveredOver == 0 && !backButtonRect.Contains(mouse)) {
			currentHoveredOver = -1;
		}
	}
}
