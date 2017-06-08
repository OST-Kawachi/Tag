using UnityEngine;

namespace Player.Ogre{

	/// <summary>
	/// 鬼側のプレイヤー制御
	/// </summary>
	public class OgreController : MonoBehaviour {
		
		/// <summary>
		/// 接触イベント
		/// </summary>
		/// <param name="other">このオブジェクトに触れたプレイヤー</param>
		private void OnTriggerEnter( Collider other ) {
			
			//触れたのが逃げる側のプレイヤーだった場合
			if( other.tag == "Escape" ) {

				PlayerController myController = this.gameObject.GetComponent<PlayerController>();
				PlayerController otherController = other.GetComponent<PlayerController>();

				//無敵状態でない場合
				if( !otherController.EscapeController.IsInvincible ) {
					myController.BecomeEscape();
					otherController.BecomeOgre();
				}

			}

		}

	}

}