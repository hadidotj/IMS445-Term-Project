using UnityEngine;
using System.Collections;

public class OptionsMenu : AbstractMenu {

	public static readonly string QUALITY_SETTING_USER_PREF_KEY = "QualitySettings";
	public static readonly string RESOLUTION_WIDTH_UPK = "ResolutionWidth";
	public static readonly string RESOLUTION_HEIGHT_UPK = "ResolutionHeight";
	public static readonly string FULL_SCREEN_UPK = "FullScreen";

	public Texture2D header;
	public Texture2D apply;
	public Texture2D back;

	private float volumeSetting;
	private int qualitySettingIndex;
	private int resolutionIndex;
	private bool fullScreen;

	private string[] resolutions;
	
	private int currentHoveredOver = -1;
	
	public void Start() {
		volumeSetting = SoundUtils.GLOBAL_VOLUME;
	}

	public override void OnOpen() {
		volumeSetting = SoundUtils.GLOBAL_VOLUME;
		qualitySettingIndex = QualitySettings.GetQualityLevel();

		resolutions = new string[Screen.resolutions.Length];
		for(int i=0; i<Screen.resolutions.Length; i++) {
			Resolution r = Screen.resolutions[i];
			resolutions[i] = r.width + " x " + r.height;
		}

		fullScreen = Screen.fullScreen;
	}
	
	public override void Draw(){
		Vector3 mouse = Input.mousePosition;
		mouse.y = Screen.height-mouse.y;

		// Draw header
		GUI.DrawTexture(new Rect(Screen.width/2.0f - header.width/2.0f, 10, header.width, header.height), header);

		// Volume setting
		GUI.Label(new Rect(Screen.width/2.0f-200, header.height+25, 400, 25), "Volume: " + volumeSetting);
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

		// Quality Settings
		GUI.Label(new Rect(Screen.width/2.0f-200, header.height+100, 400, 25), "Quaility Setting: " + QualitySettings.names[qualitySettingIndex]);
		qualitySettingIndex = GUI.SelectionGrid(new Rect(Screen.width/2.0f-200, header.height+125, 400, 25),
		                                        qualitySettingIndex, QualitySettings.names, QualitySettings.names.Length);

		// Resolution Settings
		GUI.Label(new Rect(Screen.width/2.0f-200, header.height+150, 400, 25), "Resolution Setting: " + Screen.currentResolution.width + " x " + Screen.currentResolution.height);
		resolutionIndex = GUI.SelectionGrid(new Rect(Screen.width/2.0f-200, header.height+175, 400, 25),
		                                    resolutionIndex, resolutions, resolutions.Length);

		// Full screen
		//GUI.Label(new Rect(Screen.width/2.0f-200, header.height+200, 400, 25), "Full Screen: " + fullScreen);
		fullScreen = GUI.Toggle(new Rect(Screen.width/2.0f-200, header.height+200, 400, 25), fullScreen, "Full Screen");

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

		QualitySettings.SetQualityLevel(qualitySettingIndex, true);
		PlayerPrefs.SetInt(QUALITY_SETTING_USER_PREF_KEY, qualitySettingIndex);

		Screen.SetResolution(
			Screen.resolutions[resolutionIndex].width,
			Screen.resolutions[resolutionIndex].height,
			fullScreen
			);

		PlayerPrefs.SetInt(RESOLUTION_WIDTH_UPK, Screen.resolutions[resolutionIndex].width);
		PlayerPrefs.SetInt(RESOLUTION_HEIGHT_UPK, Screen.resolutions[resolutionIndex].height);
		PlayerPrefs.SetInt(FULL_SCREEN_UPK, (fullScreen) ? 1 : 0);

		PlayerPrefs.Save();
	}
}
