using UnityEngine.Networking;

public class LobbyInit : NetworkBehaviour {

	void Awake () {
        GameManager.PlayerReset();
	}
	
}
