using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby {

	/// <summary>
	/// ロビー制御
	/// </summary>
	public class LobbyController : LobbyControllerBase {
		
		/// <summary>
		/// 参加前に自分のIPを表示するテキスト
		/// </summary>
		public Text myIPText;

		/// <summary>
		/// 参加前にIPを入力するInputField
		/// </summary>
		public InputField hostIpInputField;

		/// <summary>
		/// 部屋を作る／参加する前のオブジェクト
		/// </summary>
		public GameObject beforeReception;

		/// <summary>
		/// 部屋を作る／参加した後のオブジェクト
		/// </summary>
		public GameObject afterReception;

		/// <summary>
		/// 部屋名のIPアドレス表示欄
		/// </summary>
		public Text roomIpText;

		/// <summary>
		/// 参加者一覧を表示するテキスト
		/// </summary>
		public Text MemberText;

		/// <summary>
		/// 準備完了ボタン
		/// </summary>
		public Button addButton;

		/// <summary>
		/// 準備待ちに戻るボタン
		/// </summary>
		public Button waitButton;
		
		public void Awake() {

			// 自身のIPv4を取得
			this.myIPText.text = Util.GetMyIpv4() ?? "IPが取得できませんでした";

			// 部屋を作る前のオブジェクトをアクティブにする
			this.beforeReception.SetActive( true );
			this.afterReception.SetActive( false );

			this.addButton.gameObject.SetActive( true );
			this.waitButton.gameObject.SetActive( false );
			
			this.startHostAfterAction = () => {

				// オブジェクト切り替え
				this.beforeReception.SetActive( false );
				this.afterReception.SetActive( true );

				this.roomIpText.text = "部屋IP：" + this.myIPText.text;

			};

			this.startClientBeforeAction = () => {
				this.IpAddressOfRoom = this.hostIpInputField.text;
			};

			this.startClientAfterAction = () => {

				// オブジェクト切り替え
				this.beforeReception.SetActive( false );
				this.afterReception.SetActive( true );

				this.roomIpText.text = "部屋IP：" + this.hostIpInputField.text;

			};

			this.getReadyToEnterRoomClientAfterAction = () => {

				// ボタン切り替え
				this.addButton.gameObject.SetActive( false );
				this.waitButton.gameObject.SetActive( true );
				
				this.SendServerAddedToClient();

			};

			this.enterRoomHostAfterAction = () => {

				this.SendServerAddedToClient();

			};

			this.prepareToEnterRoomClientAfterAction = () => {

				this.addButton.gameObject.SetActive( true );
				this.waitButton.gameObject.SetActive( false );

				this.SendServerWaitedToClient();

			};

			this.leaveRoomHostAfterAction =
			this.leaveRoomClientAfterAction = () => {
				// オブジェクト切り替え
				this.beforeReception.SetActive( true );
				this.afterReception.SetActive( false );
			};

		}
		
		public void Update() {
			
			// プレイヤープレハブを洗い出し、参加者として表示
			// TODO もっといい方法があるはず
			GameObject[] players = GameObject.FindGameObjectsWithTag( "Player" );
			string str =
				"参加者人数：" + players.Length + "\n";
			for( int i = 0 ; i < players.Length ; i++ ) {
				str += "Player" + i + " Status : " + ( "OK".Equals( players[i].name ) ? "OK" : "WAIT" ) + "\n";
			}
			
			this.MemberText.text = str;
			
		}
		
		/// <summary>
		/// ホストで参加ボタン押下時イベント
		/// </summary>
		public void OnClickStartHostButton() {
			this.StartHost();
		}

		/// <summary>
		/// クライアントで参加ボタン押下時イベント
		/// </summary>
		public void OnClickStartClientButton() {
			this.StartClient();
		}
		
		/// <summary>
		/// 準備完了ボタン押下時イベント
		/// </summary>
		public void OnClickAddPartyButton() {
			this.AddParty();			
		}
		
		/// <summary>
		/// 準備待ちに戻るボタン押下時イベント
		/// </summary>
		public void OnClickPrepareToEnterRoomButton() {
			this.PrepareToEnterRoom();
		}

		/// <summary>
		/// 部屋から出るボタン押下時イベント
		/// </summary>
		public void OnClickLeaveRoomButton() {
			this.LeaveRoom();
		}
		
		/// <summary>
		/// クライアントが準備完了になったことをサーバに送信
		/// </summary>
		private void SendServerAddedToClient() {

			// プレイヤープレハブを探し出してOK以外のプレハブ名を一つOKにする
			GameObject[] players = GameObject.FindGameObjectsWithTag( "Player" );
			foreach( GameObject player in players ) {
				if( !"OK".Equals( player.name ) ) {
					player.name = "OK";
					break;
				}
			}

		}

		/// <summary>
		/// クライアントが準備待ちになったことをサーバに送信
		/// </summary>
		private void SendServerWaitedToClient() {
			// プレイヤープレハブを探し出してOKのプレハブ名を一つWAITにする
			GameObject[] players = GameObject.FindGameObjectsWithTag( "Player" );
			foreach( GameObject player in players ) {
				if( "OK".Equals( player.name ) ) {
					player.name = "WAIT";
					break;
				}
			}
		}

	}

}