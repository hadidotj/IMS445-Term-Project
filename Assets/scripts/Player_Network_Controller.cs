using UnityEngine;
using System.Collections;

public class Player_Network_Controller : MonoBehaviour {

	public string playerName;
	public GameObject nameTag;
	private Color onColor;
	
	public void setName(string name, Color color) {
		GetComponent<Player_Controler>().playerName = name;
		if(nameTag == null) {
			nameTag = new GameObject("Name Tag");
			nameTag.transform.parent = transform;
			GUIText tag = nameTag.AddComponent<GUIText>();
			tag.fontSize = 15;
			tag.color = color;
			tag.fontStyle = FontStyle.Bold;
			tag.alignment = TextAlignment.Center;
			tag.anchor = TextAnchor.UpperCenter;
			tag.enabled = false;
		}
		nameTag.guiText.text = name;
	}

	public void setTeam(bool t, string name) {
		networkView.RPC("RPCSetTeam", RPCMode.AllBuffered, t, name);
	}
	
	[RPC]
	public void RPCSetTeam(bool t, string name) {
		Debug.Log("Setting Team to " + t + " for " + name);
		//if(!teamSet) {
		GetComponent<Team>().teamName = ((t) ? "Green" : "Red");
		onColor = (t) ? Color.green : Color.red;
		setName(name, onColor);
		GetComponentInChildren<MeshRenderer>().materials[0].color = onColor;
		//}
	}

	public void FireMyLazer() {
		Player_Controler plc = gameObject.GetComponent<Player_Controler>();
		if(plc.getCharge() >= plc.fireCost) {
			plc.subtractCharge(plc.fireCost);
			PowerUp_Controler pc = gameObject.GetComponent<PowerUp_Controler>();
			
			Color fireColor = ((gameObject.GetComponent<Team>().teamName == "Green") ? Color.green : Color.red);
			
			if(pc.getMode() == 1) { // fire
				fireColor = new Color(1.0f, 0.627f, 0.0f);
				networkView.RPC("FireLazer", RPCMode.All, plc.fireLocation2.position, plc.fireLocation2.rotation, new Vector3(fireColor.r, fireColor.g, fireColor.b));
				// TODO remove extra charge in the player_conroler
			} else if(pc.getMode() == 2) { // water
				fireColor = Color.blue;
			} else if(pc.getMode() == 3) { // earth
				fireColor = Color.yellow;
			} else if(pc.getMode() == 4) { // air
				fireColor = Color.cyan;
			}
			networkView.RPC("FireLazer", RPCMode.All, plc.fireLocation1.position, plc.fireLocation1.rotation, new Vector3(fireColor.r, fireColor.g, fireColor.b));
		}
	}
	
	[RPC]
	public void FireLazer(Vector3 pos, Quaternion rot, Vector3 color, NetworkMessageInfo info) {
		LazerBeam.CreateLazerBeam(pos, rot, new Color(color.x, color.y, color.z, 1.0f), info.networkView.gameObject);
	}

	public void setChargedColor(bool on) {
		Color current = GetComponentInChildren<MeshRenderer>().materials[0].color;
		if(current == onColor && !on || current != onColor && on) {
			networkView.RPC("RPCSetChargedColor", RPCMode.AllBuffered, on);
		}
	}

	[RPC]
	public void RPCSetChargedColor(bool on) {
		GetComponentInChildren<MeshRenderer>().materials[0].color = (on) ? onColor : onColor*0.25f;
	}
}
