using UnityEngine;
using System.Collections;

public class AbstractMenu : MonoBehaviour {

	protected MenuManager menuManager;

	public virtual void Draw() {
		Debug.LogWarning("AbstractMenu.Draw() called though it is not implemented. Make sure " + name + " is overriding the Draw method!");
	}

	public virtual void OnOpen() { }
	
	public virtual void ButtonPressed(string buttonName = null) {
		SoundUtils.playSound(gameObject, menuManager.clickSound);
		if(buttonName != null) {
			menuManager.OpenMenu(buttonName);
		}
	}

	public void SetMenuManager(MenuManager menuManager) {
		this.menuManager = menuManager;
	}

	public void ButtonHover() {
		SoundUtils.playSound(gameObject, menuManager.hoverSound);
	}
}
