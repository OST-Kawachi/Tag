using Network.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Network.Manager {

	/// <summary>
	/// ゲーム管理
	/// </summary>
	public class GameManager : NetworkBehaviour {

		/// <summary>
		/// シングルトン用のインスタンス
		/// </summary>
		static public GameManager Instance { private set; get; }

		public Canvas canvas;
		static public List<PlayerInfo> playerList = new List<PlayerInfo>();

		public int EscapePerOni = 5;

		public int playerNum = 0;

		//public class IntList : SyncListStruct<int> { }
		//IntList oniInitList = new IntList();
		public SyncListInt oniInitList = new SyncListInt();

		static public float playTimeMax = 180f;
		private float playtime = 0;

		static public float OniTimeStart = 0;
		static public float OniTimeEnd = 0;

		private Text centerText;
		private Text timeText;
		private Text playerInfoText;

		private bool play = false;

		public void Awake() {
			Instance = this;
		}

		void Start() {
			centerText = canvas.transform.Find ("CenterText").GetComponent<Text>();
			timeText = canvas.transform.Find ("TimeText").GetComponent<Text>();
			playerInfoText = canvas.transform.Find ("PlayerInfoText").GetComponent<Text>();

			StartCoroutine( this.StartTurm() );
		}

		void Update(){
			if(play){
				playtime -= Time.deltaTime;
				TimeDisplay (playtime);
			}
			PlayerInfoDisplay ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private IEnumerator StartTurm() {
			yield return new WaitForSeconds( 1 );
			//OniSelect(OniNumGet(EscapePerOni));
			playerNum = playerList.Count;
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
			for( int i = 0 ; i < playerNum ; i++ ) {
				playerList[ i ].m_Instance.GetComponent<PlayerManagerNetwork>().boolMove = true;
			}

			StartCoroutine (PlayTurm ());
		}

		private IEnumerator PlayTurm() {
			play = true;
			playtime = playTimeMax;
			OniTimeStart = Time.realtimeSinceStartup + 15;
			yield return new WaitForSeconds(playTimeMax);
			StartCoroutine (EndTurm());
		}

		private IEnumerator EndTurm(){
			play = false;
			for( int i = 0 ; i < playerNum ; i++ ) {
				playerList[ i ].m_Instance.GetComponent<PlayerManagerNetwork>().boolMove = false;
			}
			centerText.text = "Time is up!";
			centerText.color = Color.red;
			yield return new WaitForSeconds( 1 );
			centerText.text = "";
			centerText.color = Color.black;
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
				playerList [oni].m_OniStatus = true;
			}
		}

		int OniNumGet( int num ) {
			return playerNum / num + 1;
		}

		static public void AddPlayer( GameObject player , string name , int id ) {
			PlayerInfo playerInfo = new PlayerInfo() {
				m_Instance = player ,
				m_PlayerName = name ,
				m_LocalPlayerID = id,
				m_OniNum = 0,
				m_OniTime = 0,
				m_OniStatus = false
			};

			playerList.Add( playerInfo );
		}

		static public void OniPlayerSetInfo(GameObject oniPlayer){
			OniTimeEnd = Time.realtimeSinceStartup;
			for( int i = 0 ; i < playerList.Count ; i++ ) {
				if(playerList[i].m_OniStatus){
					playerList[i].m_OniStatus = false;
					playerList [i].m_OniTime = OniTimeEnd - OniTimeStart;
				}
			}

			for( int i = 0 ; i < playerList.Count ; i++ ) {
				if(playerList[i].m_Instance == oniPlayer){
					playerList[i].m_OniNum += 1;
					playerList[i].m_OniStatus = true;
				}
			}
			OniTimeStart = Time.realtimeSinceStartup;
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
					rand = Random.Range( 0 , playerNum );
					if( playerList[ rand ].m_Instance.GetComponent<PlayerManagerNetwork>().oniSet ) { continue; }
					this.oniInitList.Add( rand );
					break;
				}
			}
		}

		static public void setTimeMax(float timeMax){
			playTimeMax = timeMax;
		}

		private void TimeDisplay(float time){
			int min = 0;
			int sec = 0;
			string timeDisp = "";

			min = Mathf.FloorToInt (time / 60);
			sec = Mathf.FloorToInt (time % 60);

			timeDisp = string.Format("{0}:{1:D2}", min, sec);

			if(time <= 60){
				timeText.color = Color.red;
			}

			if(time <= 0){
				timeDisp = "0:00";	
			}

			timeText.text = timeDisp;

		}

		private void PlayerInfoDisplay(){
			string playerInfoDisp = "";

			for( int i = 0 ; i < playerNum ; i++ ) {
				if (playerList [i].m_OniStatus) {
					playerInfoDisp += "<color=#ff0000>"
									+ playerList [i].m_PlayerName
									+ " 【鬼】 " 
									+ playerList[i].m_OniNum + "[times] "
									+ Mathf.FloorToInt(playerList[i].m_OniTime)
									+ "[sec]</color>\n";			
				} else {
					playerInfoDisp += playerList[i].m_PlayerName + " 【逃】 "
									+ playerList[i].m_OniNum + "[times] "
									+ Mathf.FloorToInt(playerList[i].m_OniTime)
									+ "[sec]\n";;
				}
			}
			playerInfoText.text = playerInfoDisp;
		}
	}

}