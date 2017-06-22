using System.Collections.Generic;
using System.Net;
using Network.Manager;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

namespace Lobby {

	/// <summary>
	/// ロビー制御
	/// </summary>
	public class LobbyController : NetworkLobbyManager {
		
		/// <summary>
		/// ロビー管理
		/// </summary>
		public NetworkLobbyManager Manager;

		/// <summary>
		/// 部屋を作るボタン
		/// </summary>
		public Button CreateButton;

		/// <summary>
		/// 参加ボタン
		/// </summary>
		public Button JoinButton;

		/// <summary>
		/// 参加状態テキスト
		/// </summary>
		public Text statusText;
		
		public void Awake() {

			GameManager.PlayerReset();

			this.Manager = this;
			
		}

		/// <summary>
		/// 部屋を作るボタン押下時イベント
		/// </summary>
		public void OnClickCreate() {

			this.statusText.text = "部屋を作ります";

			this.Manager.StartMatchMaker();
			
			this.Manager.matchMaker.CreateMatch(
				"opensesame-tech" ,
				20 ,
				true ,
				"" ,
				"" ,
				"" ,
				0 ,
				0 ,
				this.Manager.OnMatchCreate
			);
			
			
			this.statusText.text = "ホストです";

			this.CreateButton.interactable = false;
			this.JoinButton.interactable = false;

			this.Manager.StartHost();
			
		}

		/// <summary>
		/// 部屋に参加するボタン押下時にネットワーク接続して参加する
		/// </summary>
		/// <param name="success"></param>
		/// <param name="extendedInfo"></param>
		/// <param name="matches"></param>
		private void JoinCallback( bool success , string extendedInfo , List<MatchInfoSnapshot> matches ) {


			this.statusText.text = "コールバックが呼ばれました";

			this.statusText.text = "マッチ数:" + matches.Count;


			NetworkID networkId = NetworkID.Invalid;
			for( int i = 0 ; i < matches.Count ; i++ ) {
				networkId = matches[ i ].networkId;
			}

			this.Manager.matchMaker.JoinMatch(
				networkId ,
				"" ,
				"" ,
				"" ,
				0 ,
				0 ,
				this.Manager.OnMatchJoined
			);

			//this.statusText.text = "参加中です";

			this.CreateButton.interactable = false;
			this.JoinButton.interactable = false;

			this.Manager.StartClient();
			
		}

		/// <summary>
		/// 部屋に参加するボタン押下時イベント
		/// </summary>
		public void OnClickJoin() {
			
			this.statusText.text = "部屋に参加します";

			this.Manager.StartMatchMaker();
			
			this.Manager.matchMaker.ListMatches( 0 , 10 , "" , true , 0 , 0 , this.JoinCallback );

		}

		/// <summary>
		/// スタートボタン押下時イベント
		/// </summary>
		public void OnClickStart() {


			Debug.Log( this.Manager.numPlayers );

		}

		public override void OnServerConnect( NetworkConnection conn ) {
		}

	}

}