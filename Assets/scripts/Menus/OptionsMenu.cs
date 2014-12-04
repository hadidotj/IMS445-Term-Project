using UnityEngine;
using System.Collections;

public class OptionsMenu : AbstractMenu {

	public Texture2D header;
	public Texture2D apply;
	public Texture2D back;

	private float volumeSetting;
	
	private int currentHoveredOver = -1;
	
	public void Start() {
		volumeSetting = SoundUtils.GLOBAL_VOLUME;
	}

	public override void OnOpen() {
		SoundUtils.setupSoundUtils();
		volumeSetting = SoundUtils.GLOBAL_VOLUME;
	}
	
	public override void Draw(){
		Vector3 mouse = Input.mousePosition;
		mouse.y = Screen.height-mouse.y;

		// Draw header
		GUI.DrawTexture(new Rect(Screen.width/2.0f - header.width/2.0f, 10, header.width, header.height), header);

		// Volume setting
		GUI.Label(new Rect(Screen.width/2.0f-200, header.height+50, 400, 25), "Volume: " + volumeSetting);
		GUI.skin.horizontalSlider.normal.background = new Texture2D(1, 1);
		GUI.skin.horizontalSlider.normal.background.SetPixel(0, 0, new Color(0.7f, 0.7f, 0.1f, 1.0f));
		GUI.skin.horizontalSlider.normal.background.Apply();

		GUI.skin.horizontalSliderThumb.normal.background = new Texture2D(1, 1);
		GUI.skin.horizontalSliderThumb.normal.background.SetPixel(0, 0, new Color(0.3f, 0.3f, 0.0f, 1.0f));
		GUI.skin.horizontalSliderThumb.normal.background.Apply();

		GUI.skin.horizontalSliderThumb.active.background = new Texture2D(1, 1);
		GUI.skin.horizontalSliderThumb.active.background.SetPixel(0, 0, new Color(0.3f, 0.3f, 0.0f, 1.0f));
		GUI.skin.horizontalSliderThumb.active.background.Apply();

		GUI.skin.horizontalSliderThumb.hover.background = new Texture2D(1, 1);
		GUI.skin.horizontalSliderThumb.hover.background.SetPixel(0, 0, new Color(0.3f, 0.3f, 0.0f, 1.0f));
		GUI.skin.horizontalSliderThumb.hover.background.Apply();
		volumeSetting = GUI.HorizontalSlider(new Rect(Screen.width/2.0f-200, header.height+75, 400, 25), volumeSetting, 0.0f, 1.0f);

		// Go Button
		GUI.backgroundColor = Color.yellow;
		Rect connectButtonRect = new Rect(Screen.width-apply.width-10.0f, Screen.height-apply.height-10.0f, apply.width, apply.height);
		if(GUI.Button (connectButtonRect, apply)){
			ApplySettings();
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
		} else if(currentHoveredOver != 0 && backButtonRect.Contains(mouse)) {
			currentHoveredOver = 0;
			ButtonHover();
		} else if(currentHoveredOver == 0 && !backButtonRect.Contains(mouse)) {
			currentHoveredOver = -1;
		}
	}

	public void ApplySettings() {
		SoundUtils.SetGlobalVolume(volumeSetting);
		PlayerPrefs.Save();
	}
}
