using Network.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Network.GameManager {

	/// <summary>
	/// ゲーム管理
	/// </summary>
	public class GameManager : NetworkBehaviour {

		/// <summary>
		/// シングルトン用のインスタンス
		/// </summary>
		static private GameManager instance;

		public Text centerText;
		static public List<PlayerInfo> playerList = new List<PlayerInfo>();

		public int EscapePerOni = 5;

		public int playerNum = 0;

		//public class IntList : SyncListStruct<int> { }
		//IntList oniInitList = new IntList();
		public SyncListInt oniInitList = new SyncListInt();


		public void Awake() {
			instance = this;
		}

		void Start() {

			StartCoroutine( this.StartTurm() );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private IEnumerator StartTurm() {
			yield return new WaitForSeconds( 1 );
			//OniSelect(OniNumGet(EscapePerOni));
			this.playerNum = playerList.Count;
			OniInitRandom( OniNumGet( this.EscapePerOni ) );
			this.centerText.text = "鬼決定！";
			yield return new WaitForSeconds( 1 );
			OniSelect( this.oniInitList );

			this.centerText.text = "3";
			yield return new WaitForSeconds( 1 );
			this.centerText.text = "2";
			yield return new WaitForSeconds( 1 );
			this.centerText.text = "1";
			yield return new WaitForSeconds( 1 );
			this.centerText.text = "START";
			yield return new WaitForSeconds( 1 );
			this.centerText.text = "";
			for( int i = 0 ; i < this.playerNum ; i++ ) {
				playerList[ i ].m_Instance.GetComponent<PlayerManagerNetwork>().boolMove = true;
			}
		}

		/*
		void OniSelect(int oniNum) {
			int rand = 0;

			for (int i = 0; i < oniNum; i++)
			{
				while (true) {
					rand = Random.Range(0, playerNum);
					if (playerList[rand].m_Instance.GetComponent<PlayerManagerNetwork>().oniSet) { continue; }
					playerList[i].m_Instance.GetComponent<PlayerManagerNetwork>().OniSet();
					break;
				}
			}
		}
		*/

		void OniSelect( SyncListInt oniArray ) {
			foreach( int oni in oniArray ) {
				playerList[ oni ].m_Instance.GetComponent<PlayerManagerNetwork>().OniSet();
			}
		}

		int OniNumGet( int num ) {
			return this.playerNum / num + 1;
		}

		static public void AddPlayer( GameObject player , string name , int id ) {
			PlayerInfo playerInfo = new PlayerInfo() {
				m_Instance = player ,
				m_PlayerName = name ,
				m_LocalPlayerID = id
			};

			playerList.Add( playerInfo );
		}

		public void RemovePlayer( GameObject player ) {
			PlayerInfo toRemove = null;
			foreach( PlayerInfo tmp in playerList ) {
				if( tmp.m_Instance == player ) {
					toRemove = tmp;
					break;
				}
			}

			if( toRemove != null )
				playerList.Remove( toRemove );
		}

		static public void PlayerReset() {
			playerList.Clear();
		}

		[Server]
		private void OniInitRandom( int oniNum ) {
			int rand = 0;
			for( int i = 0 ; i < oniNum ; i++ ) {
				while( true ) {
					rand = Random.Range( 0 , this.playerNum );
					if( playerList[ rand ].m_Instance.GetComponent<PlayerManagerNetwork>().oniSet ) { continue; }
					this.oniInitList.Add( rand );
					break;
				}
			}
		}
	}

}