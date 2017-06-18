using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Player {

	/// <summary>
	/// プレイヤーの移動制御
	/// </summary>
	public class PlayerMoveController : MonoBehaviour {

		/// <summary>
		/// デフォルトの移動スピード
		/// </summary>
		private float defaultSpeed = 0.3f;
		/// <summary>
		/// 現状のスピード
		/// </summary>
		public float Speed { private set; get; }
		
		private void Awake() {
			this.Speed = this.defaultSpeed;
		}

		private void Update() {

#if UNITY_EDITOR
			this.UpdateAdvanceOnEditor();
			this.UpdateTurnOnEditor();
#elif UNITY_IOS
			this.UpdateAdvanceOnIOS();
			this.UpdateTurnOnIOS();
#elif UNITY_ANDROID
			this.UpdateAdvanceOnAndroid();
			this.UpdateTurnOnAndroid();
#endif

		}

		#region エディタ用制御
#if UNITY_EDITOR

		private static KeyCode moveRight = KeyCode.A;
		private static KeyCode moveLeft = KeyCode.D;
		private static KeyCode moveForWard = KeyCode.W;
		private static KeyCode moveBack = KeyCode.S;

		private static KeyCode turnRight = KeyCode.RightArrow;
		private static KeyCode turnLeft = KeyCode.LeftArrow;

		/// <summary>
		/// エディタ用前進
		/// </summary>
		private void UpdateAdvanceOnEditor() {


			float x = 0;
			float z = 0;
			if( Input.GetKey( moveLeft ) && !Input.GetKey( moveRight ) )
				x = 1.0f;
			else if( Input.GetKey( moveRight ) && !Input.GetKey( moveLeft ) )
				x = -1.0f;
			else
				x = 0;

			if( Input.GetKey( moveForWard ) && !Input.GetKey( moveBack ) )
				z = 1.0f;
			else if( Input.GetKey( moveBack ) && !Input.GetKey( moveForWard ) )
				z = -1.0f;
			else
				z = 0;

			this.UpdatePosition( new Vector3( x , 0 , z ) );
			
		}

		/// <summary>
		/// エディタ用回転
		/// </summary>
		private void UpdateTurnOnEditor() {
			
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

#endif
		#endregion

		#region IOS用制御
#if UNITY_IOS
		private void UpdateAdvanceOnIOS(){


		}
		private void UpdateTurnOnIOS(){

			Quaternion gyro = Input.gyro.attitude;
			Quaternion turn = new Quaternion (-gyro.x, -gyro.z, -gyro.y, gyro.w) * Quaternion.Euler (90f, 0f, 0f);
			//Android 用の回転
			//turn = new Quaternion (-gyro.x, gyro.y, -gyro.z, gyro.w) * Quaternion.Euler (90f, 0f, 0f);

			this.transform.localRotation = turn;
			this.transform.rotation = Quaternion.AngleAxis( turn.eulerAngles.y , new Vector3( 0 , 1 , 0 ) );

		}

#endif
		#endregion

		#region Android用制御
#if UNITY_ANDROID
		private void UpdateAdvanceOnAndroid(){

		}


		private void UpdateTurnOnAndroid(){

			Quaternion gyro = Input.gyro.attitude;
			Quaternion turn = new Quaternion (-gyro.x, gyro.y, -gyro.z, gyro.w) * Quaternion.Euler (90f, 0f, 0f);

			this.transform.localRotation = turn;
			this.transform.rotation = Quaternion.AngleAxis( turn.eulerAngles.y , new Vector3( 0 , 1 , 0 ) );

		}
		
#endif
		#endregion


		/// <summary>
		/// 座標の更新
		/// </summary>
		/// <param name="direction"></param>
		private void UpdatePosition( Vector3 direction ) {
			this.transform.position += direction.normalized * this.Speed;
		}
		

		/// <summary>
		/// ??????????????????????????????
		/// </summary>
		void CrossPlatformInputMoveTranslate() {
			// 右・左
			float x = CrossPlatformInputManager.GetAxisRaw( "Horizontal" );

			// 上・下
			float z = CrossPlatformInputManager.GetAxisRaw( "Vertical" );

			// 移動Vector
			Vector3 direction = new Vector3( x , 0 , z ).normalized;
			this.transform.position += direction * this.Speed;

		}
		
	}
	
}