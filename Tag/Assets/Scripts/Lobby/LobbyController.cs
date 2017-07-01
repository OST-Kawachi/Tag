using Common;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Lobby {

	/// <summary>
	/// ロビー制御
	/// </summary>
	public class LobbyController : MonoBehaviour {

		/// <summary>
		/// ロビー管理
		/// </summary>
		public NetworkLobbyManager networkLobbyManager;
				
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
		/// 準備完了ボタン
		/// </summary>
		public Button addButton;

		/// <summary>
		/// 準備待ちに戻るボタン
		/// </summary>
		public Button waitButton;

		/// <summary>
		/// ホストかどうか
		/// </summary>
		private bool isHost;

		public void Awake() {

			// 自身のIPv4を取得
			this.myIPText.text = Util.GetMyIpv4() ?? "IPが取得できませんでした";

			// 部屋を作る前のオブジェクトをアクティブにする
			this.beforeReception.SetActive( true );
			this.afterReception.SetActive( false );

			this.addButton.gameObject.SetActive( true );
			this.waitButton.gameObject.SetActive( false );

		}

		/// <summary>
		/// ホストになるボタン押下時イベント
		/// </summary>
		public void OnClickHostButton() {

			NetworkManager.singleton.StartHost();

			// オブジェクト切り替え
			this.beforeReception.SetActive( false );
			this.afterReception.SetActive( true );
			
			this.isHost = true;

			this.roomIpText.text = "部屋IP：" + this.myIPText.text;

		}

		/// <summary>
		/// クライアントになるボタン押下時イベント
		/// </summary>
		public void OnClickClientButton() {

			// 入力したIPアドレスを設定
			NetworkManager.singleton.networkAddress = this.hostIpInputField.text;
			NetworkManager.singleton.StartClient();

			// オブジェクト切り替え
			this.beforeReception.SetActive( false );
			this.afterReception.SetActive( true );

			this.isHost = false;

			this.roomIpText.text = "部屋IP：" + this.hostIpInputField.text;

		}

		/// <summary>
		/// 準備完了ボタン押下時イベント
		/// </summary>
		public void OnClickAddButton() {

			if( this.isHost ) {
				this.networkLobbyManager.OnLobbyServerPlayersReady();
			}
			else {
				this.networkLobbyManager.OnLobbyClientEnter();
				this.addButton.gameObject.SetActive( false );
				this.waitButton.gameObject.SetActive( true );
			}
			
		}
		
		/// <summary>
		/// 準備待ちに戻るボタン押下時イベント
		/// </summary>
		public void OnClickWaitButton() {
			this.networkLobbyManager.OnLobbyClientExit();
			this.addButton.gameObject.SetActive( true );
			this.waitButton.gameObject.SetActive( false );

		}

		/// <summary>
		/// 部屋から出るボタン押下時イベント
		/// </summary>
		public void OnClickExitButton() {

			// ネットワークの停止
			if( this.isHost )
				NetworkManager.singleton.StopHost();
			else
				NetworkManager.singleton.StopClient();

			// オブジェクト切り替え
			this.beforeReception.SetActive( true );
			this.afterReception.SetActive( false );

		}
		
	}

}