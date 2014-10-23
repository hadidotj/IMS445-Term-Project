using UnityEngine;
using System.Collections;

public class IngameMenu : AbstractMenu {

	public Texture2D header;
	public Texture2D back;

	private int currentHoveredOver = -1;
	private bool showMenu = false;

	public override void Draw(){
		if(showMenu) {
			Vector3 mouse = Input.mousePosition;
			mouse.y = Screen.height-mouse.y;

			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

			// Draw header
			GUI.DrawTexture(new Rect(Screen.width/2.0f - header.width/2.0f, 10, header.width, header.height), header);
			
			// Back Button
			GUI.backgroundColor = Color.red;
			Rect backButtonRect = new Rect(10.0f, Screen.height-back.height-10.0f, back.width, back.height);
			if(GUI.Button (backButtonRect, back)){
				ButtonPressed();
				NetworkManager.DisconnectFromServer();
			} else if(currentHoveredOver != 1 && backButtonRect.Contains(mouse)) {
				currentHoveredOver = 1;
				ButtonHover();
			} else if(currentHoveredOver == 1 && !backButtonRect.Contains(mouse)) {
				currentHoveredOver = -1;
			}
		}
	}

	public void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			showMenu = !showMenu;
		}
	}
}
