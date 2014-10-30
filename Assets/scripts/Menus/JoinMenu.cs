using UnityEngine;
using System.Collections;

public class JoinMenu : AbstractMenu {

	public Texture2D header;
	public Texture2D back;
	public Texture2D connect;
	
	private int currentHoveredOver = -1;
	private string joinIP = "localhost";
	
	public override void Draw(){
		Vector3 mouse = Input.mousePosition;
		mouse.y = Screen.height-mouse.y;
		
		// Draw header
		GUI.Label(new Rect(Screen.width/2.0f - header.width/2.0f, 10, header.width, header.height), header);

		// IP Input
		joinIP = GUI.TextField(new Rect(Screen.width/2.0f-200, header.height+50, 400, 25), joinIP);

		// Go Button
		GUI.backgroundColor = Color.green;
		Rect connectButtonRect = new Rect(Screen.width-connect.width-10.0f, Screen.height-connect.height-10.0f, connect.width, connect.height);
		if(GUI.Button (connectButtonRect, connect)){
			NetworkManager.ConnectToServer(joinIP);
			ButtonPressed("MainMenu");
		} else if(currentHoveredOver != 0 && connectButtonRect.Contains(mouse)) {
			currentHoveredOver = 0;
			ButtonHover();
		} else if(currentHoveredOver == 0 && !connectButtonRect.Contains(mouse)) {
			currentHoveredOver = -1;
		}
		
		// Back Button
		GUI.backgroundColor = Color.red;
		Rect backButtonRect = new Rect(10.0f, Screen.height-back.height-10.0f, back.width, back.height);
		if(GUI.Button (backButtonRect, back)){
			ButtonPressed("MainMenu");
		} else if(currentHoveredOver != 1 && backButtonRect.Contains(mouse)) {
			currentHoveredOver = 1;
			ButtonHover();
		} else if(currentHoveredOver == 1 && !backButtonRect.Contains(mouse)) {
			currentHoveredOver = -1;
		}
	}
}
