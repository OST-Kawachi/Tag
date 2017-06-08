using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Player {

	public class PlayerMove : MonoBehaviour {
		private float speed = 0.3f;

		private static KeyCode moveRight = KeyCode.A;
		private static KeyCode moveLeft = KeyCode.D;
		private static KeyCode moveForWard = KeyCode.W;
		private static KeyCode moveBack = KeyCode.S;

		private static KeyCode turnRight = KeyCode.RightArrow;
		private static KeyCode turnLeft = KeyCode.LeftArrow;

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {
			InputKeyMoveTranslate();
			InputKeyTurn();
			CrossPlatformInputMoveTranslate();
		}

		void InputKeyMoveTranslate() {
			float x = 0;
			float z = 0;
			if( Input.GetKey( moveLeft ) && !Input.GetKey( moveRight ) ) {
				x = 1.0f;

			}
			else if( Input.GetKey( moveRight ) && !Input.GetKey( moveLeft ) ) {
				x = -1.0f;
			}
			else {
				x = 0;
			}

			if( Input.GetKey( moveForWard ) && !Input.GetKey( moveBack ) ) {
				z = 1.0f;
			}
			else if( Input.GetKey( moveBack ) && !Input.GetKey( moveForWard ) ) {
				z = -1.0f;
			}
			else {
				z = 0;
			}

			Vector3 direction = new Vector3( x , 0 , z ).normalized;
			TranslateMove( direction );
		}

		void CrossPlatformInputMoveTranslate() {
			// 右・左
			float x = CrossPlatformInputManager.GetAxisRaw( "Horizontal" );

			// 上・下
			float z = CrossPlatformInputManager.GetAxisRaw( "Vertical" );

			// 移動Vector
			Vector3 direction = new Vector3( x , 0 , z ).normalized;

			TranslateMove( direction );
		}

		private void TranslateMove( Vector3 direction ) {
			direction.x *= this.speed;
			direction.z *= this.speed;
			this.transform.position += direction;
		}

		void InputKeyTurn() {
			float turn = 0;

			if( Input.GetKey( turnRight ) && !Input.GetKey( turnLeft ) ) {
				turn = 1f;
			}
			else if( Input.GetKey( turnLeft ) && !Input.GetKey( turnRight ) ) {
				turn = -1;
			}
			else {
				turn = 0;
			}

			this.transform.Rotate( 0 , turn , 0 );
		}

	}


}