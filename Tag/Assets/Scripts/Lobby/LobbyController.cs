using System.Net;
using Network.Manager;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Lobby {

	/// <summary>
	/// ロビー制御
	/// </summary>
	public class LobbyController : NetworkBehaviour {
		
		/// <summary>
		/// ロビー管理
		/// </summary>
		public NetworkLobbyManager Manager;

		/// <summary>
		/// 自分のIPを表示するテキスト
		/// </summary>
		public Text MyIPText;

		public void Awake() {

			GameManager.PlayerReset();

			this.Manager = this.gameObject.GetComponent<NetworkLobbyManager>();

			string hostName = Dns.GetHostName();
			IPAddress[] addresses = Dns.GetHostAddresses( hostName );
			foreach( IPAddress address in addresses ) {
				this.MyIPText.text = "自分のIP：" + address.ToString();
			}
			
		}

	}

}