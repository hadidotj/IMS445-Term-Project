using UnityEngine;
using System.Collections;

public class LazerTagGametype : MonoBehaviour {

	public string getName() {
		return name;
	}

	public virtual int getTeamScore(string teamName) {
		return 0;
	}

	public virtual bool hasTeams() {
		return true;
	}
}
