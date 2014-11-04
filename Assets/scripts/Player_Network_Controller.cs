using UnityEngine;
using System.Collections;

public class Player_Network_Controller : MonoBehaviour {

	public void setTeam(bool t, string name) {
		networkView.RPC("RPCSetTeam", RPCMode.AllBuffered, t, name);
	}
	
	[RPC]
	public void RPCSetTeam(bool t, string name) {
		Debug.Log("Setting Team to " + t + " for " + name);
		//if(!teamSet) {
		GetComponent<Team>().teamName = ((t) ? "Green" : "Red");
		GetComponent<Player_Controler>().playerName = name;
		GetComponentInChildren<MeshRenderer>().materials[0].color = (t) ? Color.green : Color.red;
		//}
	}

	public void FireMyLazer() {
		Player_Controler plc = gameObject.GetComponent<Player_Controler>();
		if(plc.getCharge() >= 10.0) {
			plc.subtractCharge(10.0f);
			PowerUp_Controler pc = gameObject.GetComponent<PowerUp_Controler>();
			
			Color fireColor = ((gameObject.GetComponent<Team>().teamName == "Green") ? Color.green : Color.red);
			
			if(pc.getMode() == 1) { // fire
				fireColor = new Color(1.0f, 0.627f, 0.0f);
				networkView.RPC("FireLazer", RPCMode.All, plc.fireLocation2.position, plc.fireLocation2.rotation, fireColor);
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
}
