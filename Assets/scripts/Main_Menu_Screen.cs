using UnityEngine;
using System.Collections;

public class Main_Menu_Screen : MonoBehaviour {
	// We could totally just use different objects for the screens!

	[System.Serializable]
	public class ButtonInfo {
		public Texture2D texture;
		public string levelName;
		public Color color;
		public int bottomPadding = 10;
	}

	public ButtonInfo[] buttons;
	public AudioClip hoverSound;
	public AudioClip clickSound;

	private int currentHoveredOver = -1;

	public void OnGUI(){
		float centerX = Screen.width/2.0f;
		float buttonsHeight = 0.0f;

		foreach(ButtonInfo info in buttons) {
			buttonsHeight += info.texture.height+info.bottomPadding;
		}

		float currentY = (Screen.height/2.0f) - (buttonsHeight/2.0f);

		for(int i=0; i<buttons.Length; i++) {
			ButtonInfo buttonInfo = buttons[i];
			Texture2D texture = buttonInfo.texture;

			GUI.backgroundColor = buttonInfo.color;
			Rect buttonRect = new Rect(centerX-(texture.width/2.0f), currentY, texture.width, texture.height);
			if(GUI.Button (buttonRect, texture)) {
				buttonPress(buttonInfo.levelName);
			}
			if(currentHoveredOver != i && buttonRect.Contains(Input.mousePosition)) {
				currentHoveredOver = i;
				SoundUtils.playSound(gameObject, hoverSound);
			}
			currentY += texture.height + buttonInfo.bottomPadding;
		}
	}

	private void buttonPress(string levelName) {
		SoundUtils.playSound(gameObject, clickSound);
		if(levelName == "Quit") {
			Application.Quit();
			return;
		}
		Application.LoadLevel(levelName);
	}
}
