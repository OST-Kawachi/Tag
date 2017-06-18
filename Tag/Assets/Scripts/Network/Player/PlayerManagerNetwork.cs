using Network.Manager;
using UnityEngine;
using UnityEngine.Networking;

namespace Network.Player {

	public class PlayerManagerNetwork : NetworkBehaviour {
		public GameObject oniObject;
		public GameObject escapeObject;
		public Color oniColor;
		public Color escapeColor;

		[SyncVar]
		public bool oniSet = false;
		public int playerUniqueNumber = 0;
		[SyncVar]
		public bool boolMove = false;

		public void Awake() {
			GameManager.AddPlayer( this.gameObject , "player" , 1 );
		}

		public void Start() {
			InitColorSet();
			ObjectSet();
			if( !this.isLocalPlayer ) {
				this.gameObject.transform.Find( "PlayerCamera" ).gameObject.SetActive( false );
			}

		}

		void OnControllerColliderHit( ControllerColliderHit hit ) {
			//Debug.Log(hit.gameObject.name);
			if( this.gameObject.tag == "Oni" ) {
				if( hit.gameObject.tag == "Escape" ) {
					//EscapeSet();
					//hit.gameObject.GetComponent<PlayerManagerNetwork>().OniSet();
					RpcOniTouch( hit.gameObject );
				}
			}
		}

		[ClientRpc]
		public void RpcOniTouch( GameObject escape ) {
			EscapeSet();
			escape.GetComponent<PlayerManagerNetwork>().OniSet();
			GameManager.OniPlayerSetInfo (escape);
		}

		public void OniSet() {
			this.oniSet = true;
			this.oniObject.SetActive( true );
			this.escapeObject.SetActive( false );
			this.gameObject.tag = "Oni";
		}

		public void EscapeSet() {
			this.oniSet = false;
			this.oniObject.SetActive( false );
			this.escapeObject.SetActive( true );
			this.gameObject.tag = "Escape";
		}

		void ColorSet( GameObject gameObject , Color color ) {
			MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();

			foreach( Renderer renderer in renderers ) {
				renderer.material.color = color;
			}
		}

		void InitColorSet() {
			ColorSet( this.oniObject , this.oniColor );
			ColorSet( this.escapeObject , this.escapeColor );
		}

		void ObjectSet() {
			if( this.oniSet ) {
				OniSet();
			}
			else {
				EscapeSet();
			}
		}


		public override void OnNetworkDestroy() {
			Debug.Log( "destroy" + this.gameObject.name );
			GameManager.Instance.RemovePlayer( this.gameObject );
		}
	}

}