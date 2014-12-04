using UnityEngine;
using System.Collections;

public class MainMenu : AbstractMenu {
	[System.Serializable]
	public class ButtonInfo {
		public Texture2D texture;
		public string buttonName;
		public Color color;
		public int bottomPadding = 10;
	}
	
	public ButtonInfo[] buttons;

	private int currentHoveredOver = -1;

	public void Awake() {
		SoundUtils.setupSoundUtils();
		if(PlayerPrefs.HasKey(OptionsMenu.QUALITY_SETTING_USER_PREF_KEY)) {
			QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(OptionsMenu.QUALITY_SETTING_USER_PREF_KEY, 0), true);
		}
		if(PlayerPrefs.HasKey(OptionsMenu.RESOLUTION_WIDTH_UPK) && PlayerPrefs.HasKey(OptionsMenu.RESOLUTION_HEIGHT_UPK)) {
			Screen.SetResolution(
				PlayerPrefs.GetInt(OptionsMenu.RESOLUTION_WIDTH_UPK),
				PlayerPrefs.GetInt(OptionsMenu.RESOLUTION_HEIGHT_UPK),
				PlayerPrefs.GetInt(OptionsMenu.FULL_SCREEN_UPK, 1) == 1
				);
		}
	}

	public override void Draw(){
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
				ButtonPressed(buttonInfo.buttonName);
			}
			if(currentHoveredOver != i && buttonRect.Contains(Input.mousePosition)) {
				currentHoveredOver = i;
				ButtonHover();
			} else if(currentHoveredOver == i && !buttonRect.Contains(Input.mousePosition)) {
				currentHoveredOver = -1;
			}
			currentY += texture.height + buttonInfo.bottomPadding;
		}
	}
}