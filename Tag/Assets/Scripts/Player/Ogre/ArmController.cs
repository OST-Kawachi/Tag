using System.Collections;
using UnityEngine;

namespace Player.Ogre {

	/// <summary>
	/// 腕の制御
	/// </summary>
	public class ArmController : MonoBehaviour {

		private float stretch = 0;
		private GameObject parentArm;
		private bool keyStatus = true;
		private bool stretchStatus = false;

		// Use this for initialization
		void Start() {
			this.parentArm = this.transform.root.gameObject;
		}

		// Update is called once per frame
		void Update() {

			if( this.stretch > 0.2 ) {
				this.transform.position = this.parentArm.transform.position;
				this.transform.localScale = this.parentArm.transform.localScale;
				this.stretch = 0;
				this.stretchStatus = false;
				StartCoroutine( CoolTime() );
				return;
			}

			if( Input.GetKey( KeyCode.UpArrow ) && this.keyStatus ) {
				this.stretchStatus = true;
				this.keyStatus = false;
			}

			if( this.stretchStatus ) {
				this.transform.Translate( 0 , 0 , this.stretch , Space.Self );
				this.transform.localScale += new Vector3( 0 , 0 , this.stretch );
				this.stretch += 0.02f;
			}
		}

		IEnumerator CoolTime() {
			yield return new WaitForSeconds( 2 );
			this.keyStatus = true;
		}
	}


}