using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyInit : NetworkBehaviour {

	void Awake () {
        GameManager.PlayerReset();
	}
	
}
