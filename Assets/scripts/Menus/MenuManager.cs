using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public static MenuManager instance;

	public AudioClip hoverSound;
	public AudioClip clickSound;
	public AbstractMenu[] menus;

	private string dialogMsg = null;
	private string dismissMenu = null;
	private AbstractMenu activeMenu = null;
	
	public void Start() {
		instance = this;
		GameObject.DontDestroyOnLoad(gameObject);
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
			if(dismissMenu != null && GUI.Button(new Rect(Screen.width/2.0f-75.0f, Screen.height/2.0f-50.0f+100.0f-30.0f, 150.0f, 25.0f), "OK")) {
				OpenMenu(dismissMenu);
				dismissMenu = dialogMsg = null;
			}
			Screen.lockCursor = false; // Force cursor to show!
		} else if(activeMenu != null) {
			activeMenu.Draw();
		}
	}

	public static void Open_Menu(string name) {
		instance.OpenMenu(name);
	}

	public void OpenMenu(string name) {
		if(name == "QUIT") {
			Application.Quit();
			return;
		} else if(name == "CLOSE") {
			activeMenu = null;
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

	public static void DisplayDialogBox(string msg, string dismissMenu = null) {
		instance.dialogMsg = msg;
		instance.dismissMenu = dismissMenu;
	}
}
