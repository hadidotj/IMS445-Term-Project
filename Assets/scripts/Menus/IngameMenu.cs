using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngameMenu : AbstractMenu {

	public Texture2D header;
	public Texture2D back;

	private int currentHoveredOver = -1;
	private bool showMenu = false;
	private List<GUIMessage> msgs = new List<GUIMessage>();

	private class GUIMessage {
		public string text;
		public Color color;
		public GUIMessage(string text, Color color) {
			this.text = text;
			this.color = color;
		}
	}

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
		} else if(msgs.Count > 0) {
			for(int i=0; i<msgs.Count; i++) {
				GUIStyle style = new GUIStyle();
				style.normal.textColor = msgs[i].color;
				style.fontSize = 18;
				style.alignment = TextAnchor.MiddleRight;
				GUI.Label(new Rect(5, i*20, Screen.width-10, 20), msgs[i].text, style);
			}
		}
	}

	public IEnumerator Message(object[] args) {
		string msg = (string) args[0];
		Color color = (Color) args[1];

		GUIMessage guimsg = new GUIMessage(msg, color);
		if(msgs.Count == 5) {
			msgs.RemoveAt(0);
		}
		msgs.Add(guimsg);
		yield return new WaitForSeconds(30.0f);
		msgs.Remove(guimsg);
	}

	public void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			showMenu = !showMenu;
		}
	}
}
