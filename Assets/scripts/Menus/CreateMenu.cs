using UnityEngine;
using System.Collections;

public class CreateMenu : AbstractMenu {

	public Texture2D header;
	public Texture2D back;
	public Texture2D create;

	private int currentHoveredOver = -1;

	private int selectedMap = 0;
	private string[] maps = {"Ice_Cap", "Mine", "power_ups", "Ice_Cap ctf"};

	public override void Draw(){
		Vector3 mouse = Input.mousePosition;
		mouse.y = Screen.height-mouse.y;

		// Draw header
		GUI.DrawTexture(new Rect(Screen.width/2.0f - header.width/2.0f, 10, header.width, header.height), header);

		// Player Name
		NetworkManager.playerName = GUI.TextField(new Rect(Screen.width/2.0f-200, header.height+50, 400, 25), NetworkManager.playerName);

		// Level Selection
		selectedMap = GUI.SelectionGrid (new Rect(Screen.width/2.0f-200, header.height+100, 400, 100), selectedMap, maps, 4);

		// Go Button
		GUI.backgroundColor = Color.blue;
		Rect connectButtonRect = new Rect(Screen.width-create.width-10.0f, Screen.height-create.height-10.0f, create.width, create.height);
		if(GUI.Button (connectButtonRect, create)){
			NetworkManager.CreateServer(maps[selectedMap]);
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
