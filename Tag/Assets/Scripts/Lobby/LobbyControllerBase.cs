using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Lobby {

	/// <summary>
	/// 
	/// </summary>
	public class LobbyControllerBase : MonoBehaviour {

		/// <summary>
		/// Netwrok Lobby Manager
		/// </summary>
		[SerializeField]
		protected NetworkLobbyManager networkLobbyManager;

		/// <summary>
		/// 状態
		/// </summary>
		public enum StatusEnum {

			/// <summary>
			/// 参加していない
			/// </summary>
			NotJoining ,

			/// <summary>
			/// ホストとして参加している
			/// </summary>
			JoinHost ,

			/// <summary>
			/// クライアントとして参加している
			/// </summary>
			JoinClient ,

			/// <summary>
			/// 部屋に入るのを待っている
			/// </summary>
			GetReadyToEnterRoomClient ,
			
		}

		/// <summary>
		/// 状態
		/// </summary>
		private StatusEnum status = StatusEnum.NotJoining;

		/// <summary>
		/// 状態
		/// </summary>
		public StatusEnum Status {
			private set {
				this.status = value;
			}
			get {
				return this.status;
			}
		}

		/// <summary>
		/// 部屋のIPアドレス
		/// </summary>
		public string IpAddressOfRoom { protected set; get; }

		/// <summary>
		/// ホストとしてマルチプレイに参加する前に呼ばれるコールバック関数
		/// </summary>
		public Action startHostBeforeAction = null;

		/// <summary>
		/// ホストとしてマルチプレイに参加した後に呼ばれるコールバック関数
		/// </summary>
		public Action startHostAfterAction = null;

		/// <summary>
		/// クライアントとしてマルチプレイに参加する前に呼ばれるコールバック関数
		/// </summary>
		public Action startClientBeforeAction = null;

		/// <summary>
		/// クライアントとしてマルチプレイに参加した後に呼ばれるコールバック関数
		/// </summary>
		public Action startClientAfterAction = null;

		/// <summary>
		/// ホストとして部屋に入る前に呼ばれるコールバック関数
		/// </summary>
		public Action enterRoomHostBeforeAction = null;

		/// <summary>
		/// ホストとして部屋に入った後に呼ばれるコールバック関数
		/// </summary>
		public Action enterRoomHostAfterAction = null;

		/// <summary>
		/// クライアントとして部屋に入る前に呼ばれるコールバック関数
		/// </summary>
		public Action getReadyToEnterRoomClientBeforeAction = null;

		/// <summary>
		/// クライアントとして部屋に入った後に呼ばれるコールバック関数
		/// </summary>
		public Action getReadyToEnterRoomClientAfterAction = null;

		/// <summary>
		/// クライアントとして部屋から出る前に呼ばれるコールバック関数
		/// </summary>
		public Action prepareToEnterRoomClientBeforeAction = null;

		/// <summary>
		/// クライアントとして準備待ちに戻った後に呼ばれるコールバック関数
		/// </summary>
		public Action prepareToEnterRoomClientAfterAction = null;
		
		/// <summary>
		/// ホストとして部屋から出る前に呼ばれるコールバック関数
		/// </summary>
		public Action leaveRoomHostBeforeAction = null;

		/// <summary>
		/// ホストとして部屋から出た後に呼ばれるコールバック関数
		/// </summary>
		public Action leaveRoomHostAfterAction = null;

		/// <summary>
		/// クライアントとして部屋から出る前に呼ばれるコールバック関数
		/// </summary>
		public Action leaveRoomClientBeforeAction = null;

		/// <summary>
		/// クライアントとして部屋から出た後に呼ばれるコールバック関数
		/// </summary>
		public Action leaveRoomClientAfterAction = null;

		/// <summary>
		/// ホストとしてマルチプレイに参加する
		/// </summary>
		protected void StartHost() {

			if( this.Status == StatusEnum.NotJoining ) {

				// コールバック
				if( this.startHostBeforeAction != null )
					this.startHostBeforeAction();

				// ホストとして参加
				NetworkManager.singleton.StartHost();
				this.Status = StatusEnum.JoinHost;

				// コールバック
				if( this.startHostAfterAction != null )
					this.startHostAfterAction();

			}

		}

		/// <summary>
		/// クライアントとしてマルチプレイに参加する
		/// </summary>
		protected void StartClient() {

			if( this.Status == StatusEnum.NotJoining ) {

				// コールバック
				if( this.startClientBeforeAction != null )
					this.startClientBeforeAction();

				// クライアントとして参加
				NetworkManager.singleton.networkAddress = this.IpAddressOfRoom;
				NetworkManager.singleton.StartClient();
				this.Status = StatusEnum.JoinClient;

				// コールバック
				if( this.startClientAfterAction != null )
					this.startClientAfterAction();

			}

		}

		/// <summary>
		/// 参加準備を完了する
		/// </summary>
		protected void AddParty() {

			// ホストで参加している場合はゲームを開始する
			if( this.Status == StatusEnum.JoinHost ) {

				// コールバック
				if( this.enterRoomHostBeforeAction != null )
					this.enterRoomHostBeforeAction();

				// ゲームの開始
				this.networkLobbyManager.OnLobbyServerPlayersReady();

				// コールバック
				if( this.enterRoomHostAfterAction != null )
					this.enterRoomHostAfterAction();

			}
			// クライアントで参加している場合はゲーム開始待ち状態にする
			else if( this.Status == StatusEnum.JoinClient ) {

				// コールバック
				if( this.getReadyToEnterRoomClientBeforeAction != null )
					this.getReadyToEnterRoomClientBeforeAction();

				// 開始待ち状態
				this.networkLobbyManager.OnLobbyClientEnter();
				this.Status = StatusEnum.GetReadyToEnterRoomClient;

				// コールバック
				if( this.getReadyToEnterRoomClientAfterAction != null )
					this.getReadyToEnterRoomClientAfterAction();

			}
			
		}

		/// <summary>
		/// 準備待ちに戻る
		/// </summary>
		protected void PrepareToEnterRoom() {

			if( this.Status == StatusEnum.GetReadyToEnterRoomClient ) {

				// コールバック
				if( this.prepareToEnterRoomClientBeforeAction != null )
					this.prepareToEnterRoomClientBeforeAction();

				this.networkLobbyManager.OnLobbyClientExit();

				// コールバック
				if( this.prepareToEnterRoomClientAfterAction != null )
					this.prepareToEnterRoomClientAfterAction();

			}

		}

		/// <summary>
		/// 部屋から出る
		/// </summary>
		protected void LeaveRoom() {
			
			if( this.Status == StatusEnum.JoinHost ) { 

				// コールバック
				if( this.leaveRoomHostBeforeAction != null )
					this.leaveRoomHostBeforeAction();

				// ネットワークの停止
				NetworkManager.singleton.StopHost();
				this.Status = StatusEnum.NotJoining;

				// コールバック
				if( this.leaveRoomHostAfterAction != null )
					this.leaveRoomHostAfterAction();

			}
			else if( this.Status == StatusEnum.JoinClient ) { 

				// コールバック
				if( this.leaveRoomClientBeforeAction != null )
					this.leaveRoomClientBeforeAction();

				// ネットワークの停止
				NetworkManager.singleton.StopClient();
				this.Status = StatusEnum.NotJoining;

				// コールバック
				if( this.leaveRoomClientAfterAction != null )
					this.leaveRoomClientAfterAction();

			}

		}
		
	}

}
