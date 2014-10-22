using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	private static string dialogMsg = null;

	public AudioClip hoverSound;
	public AudioClip clickSound;
	public AbstractMenu[] menus;

	private AbstractMenu activeMenu = null;

	public void Start() {
		Application.runInBackground = true;
		if(menus != null && menus.Length > 0 && menus[0] != null) {
			activeMenu = menus[0];
			foreach(AbstractMenu menu in menus) {
				menu.SetMenuManager(this);
			}
		}
	}

	public void OnGUI() {
		if(dialogMsg != null) {
			GUI.Box(new Rect(Screen.width/2.0f-100.0f, Screen.height/2.0f-50.0f,200.0f,100.0f), dialogMsg);
			if(GUI.Button(new Rect(Screen.width/2.0f-75.0f, Screen.height/2.0f-50.0f+100.0f-30.0f, 150.0f, 25.0f), "OK")) {
				dialogMsg = null;
			}
		} else if(activeMenu != null) {
			activeMenu.Draw();
		}
	}

	public void OpenMenu(string name) {
		if(name == "QUIT") {
			Application.Quit();
			return;
		}

		foreach(AbstractMenu menu in menus) {
			if(menu.GetType().Name == name) {
				activeMenu = menu;
				return;
			}
		}

		Debug.LogError("Could not find menu with name '" + name + "'! Called from menu: " + activeMenu.name);
	}

	public static void DisplayDialogBox(string msg) {
		dialogMsg = msg;
	}
}
