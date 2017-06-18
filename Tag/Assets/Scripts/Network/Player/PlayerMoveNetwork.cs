using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

namespace Network.Player {

	public class PlayerMoveNetwork : NetworkBehaviour {
		public float speed = 10.0f;
		public float turnSpeed = 5.0f;
		public float gravity = -10.0f;
		public float jumpPower = 10.0f;

		private PlayerManagerNetwork pmn;
		private static KeyCode turnRight = KeyCode.J;
		private static KeyCode turnLeft = KeyCode.K;

		private CharacterController charaCtrl;
		private bool jump = false;

		public GameObject playerObject;

		private Vector3 moveDirection = Vector3.zero;

		// Use this for initialization
		void Start() {
			this.charaCtrl = this.gameObject.GetComponent<CharacterController>();
			this.pmn = GetComponent<PlayerManagerNetwork>();
		}

		// Update is called once per frame
		void Update() {
			if (this.pmn.boolMove) {
				if (charaCtrl.isGrounded) {
					moveDirection = Vector3.zero;
					if (this.isLocalPlayer) {
						InputKeyMove ();
						InputKeyTurn ();
						CrossPlatformInputMoveTranslate ();
						CrossPlatformInputJump ();
					}
				} else {
					if (jump) {
						this.charaCtrl.Move (this.playerObject.transform.up * jumpPower * Time.deltaTime 
							+ moveDirection * this.speed * Time.deltaTime);
					} else {
						GravitySet ();
					}
				}
			}
		}

		void InputKeyMove() {
			float x = Input.GetAxis( "Horizontal" );
			float z = Input.GetAxis( "Vertical" );

			// 移動Vector
			Vector3 direction = this.transform.right * x + this.transform.forward * z;
			//TranslateMove(direction);
			this.charaCtrl.Move( direction * this.speed * 0.1f );
		}

		void CrossPlatformInputMoveTranslate() {
			// 右・左
			float x = CrossPlatformInputManager.GetAxisRaw( "Horizontal" );

			// 上・下
			float z = CrossPlatformInputManager.GetAxisRaw( "Vertical" );

			// 移動Vector
			moveDirection = this.playerObject.transform.right * x + this.playerObject.transform.forward * z;
			//TranslateMove(direction);
			this.charaCtrl.Move( moveDirection * this.speed * Time.deltaTime);
		}

		void CrossPlatformInputJump(){
			jump = CrossPlatformInputManager.GetButton ("Jump");
			if(jump){
				StartCoroutine (Jump ());
			}
		}

		void GravitySet(){
			this.charaCtrl.Move (this.playerObject.transform.up * gravity * Time.deltaTime 
				+ moveDirection * this.speed * Time.deltaTime);
		}

		void InputKeyTurn() {
			float turn = 0;

			if( Input.GetKey( turnRight ) && !Input.GetKey( turnLeft ) ) {
				//turn = 1f;
				turn = -1;
			}
			else if( Input.GetKey( turnLeft ) && !Input.GetKey( turnRight ) ) {
				//turn = -1;
				turn = 1;
			}
			else {
				turn = 0;
			}

			//transform.Rotate(0, turn * turnSpeed, 0);
			this.transform.rotation *= Quaternion.AngleAxis( this.turnSpeed , new Vector3( 0 , turn , 0 ) );
		}

		IEnumerator Jump() {
			this.charaCtrl.Move (this.playerObject.transform.up * jumpPower * Time.deltaTime 
				+ moveDirection * this.speed * Time.deltaTime);
			yield return new WaitForSeconds (0.5f);
			jump = false;
		}
	}
}