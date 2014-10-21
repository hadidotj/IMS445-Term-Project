using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public AudioClip hoverSound;
	public AudioClip clickSound;
	public AbstractMenu[] menus;

	private AbstractMenu activeMenu = null;

	public void Start() {
		if(menus != null && menus.Length > 0 && menus[0] != null) {
			activeMenu = menus[0];
			foreach(AbstractMenu menu in menus) {
				menu.SetMenuManager(this);
			}
		}
	}

	public void OnGUI() {
		if(activeMenu != null) {
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
}
