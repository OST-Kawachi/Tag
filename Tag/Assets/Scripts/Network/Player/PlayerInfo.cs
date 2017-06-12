﻿using System;
using UnityEngine;

namespace Network.Player {

	[Serializable]
	public class PlayerInfo {
		public GameObject m_Instance;             // A reference to the instance of the tank when it is created
		public string m_PlayerName;                    // The player name set in the lobby
		public int m_LocalPlayerID;                    // The player localID (if there is more than 1 player on the same machine)

	}

}