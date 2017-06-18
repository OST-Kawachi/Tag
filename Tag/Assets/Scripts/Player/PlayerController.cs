using Common;
using Player.Escape;
using Player.Ogre;
using UnityEngine;

namespace Player {

	/// <summary>
	/// プレイヤー制御
	/// </summary>
	public class PlayerController : MonoBehaviour {
		
		/// <summary>
		/// 鬼の色
		/// </summary>
		public Color OgreColor;

		/// <summary>
		/// 逃げる人の色
		/// </summary>
		public Color EscapeColor;

		/// <summary>
		/// 鬼用のコントローラ
		/// </summary>
		public OgreController OgreController;

		/// <summary>
		/// 逃げる用のコントローラ
		/// </summary>
		public EscapeController EscapeController;
		
		/// <summary>
		/// 鬼になる
		/// </summary>
		public void BecomeOgre() {
			this.ChangeMyObjectColor( true );
			this.OgreController.enabled = true;
			this.EscapeController.enabled = false;
			this.gameObject.tag = Const.TagName.Ogre;
		}

		/// <summary>
		/// 逃げる側になる
		/// </summary>
		public void BecomeEscape() {
			this.ChangeMyObjectColor( false );
			this.OgreController.enabled = false;
			this.EscapeController.enabled = true;
			this.gameObject.tag = Const.TagName.Escape;
		}

		/// <summary>
		/// オブジェクトの色を変更する
		/// </summary>
		/// <param name="isOgreColor">鬼の色にするかどうか</param>
		private void ChangeMyObjectColor( bool isOgreColor ) {
			foreach( Transform child in this.gameObject.transform ) {
				//当たり判定を持つオブジェクトの色を変更する
				if( child.GetComponent<Rigidbody>() != null ) {
					Util.SetColor( child.gameObject , isOgreColor ? this.OgreColor : this.EscapeColor );
				}
			}
		}

	}

}