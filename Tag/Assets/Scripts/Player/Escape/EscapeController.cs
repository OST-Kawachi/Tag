using System.Collections;
using UnityEngine;

namespace Player.Escape {

	/// <summary>
	/// 逃げる側のプレイヤー制御
	/// </summary>
	public class EscapeController : MonoBehaviour {

		/// <summary>
		/// 無敵状態かどうか
		/// </summary>
		[SerializeField]
		private bool isInvincible = false;

		/// <summary>
		/// 無敵状態かどうか
		/// </summary>
		public bool IsInvincible {
			private set {
				this.isInvincible = value;
			}
			get {
				return this.isInvincible;
			}
		}


		/// <summary>
		/// ComponentがActiveになる度に呼ばれる
		/// </summary>
		public void OnEnable() {

			//無敵になる
			this.BecameInvincible();

		}

		/// <summary>
		/// 無敵状態にする
		/// </summary>
		public void BecameInvincible() {
			this.isInvincible = true;
			StartCoroutine( CoolTime() );
		}

		/// <summary>
		/// 指定時間後無敵状態でなくなる
		/// </summary>
		/// <returns></returns>
		private IEnumerator CoolTime() {
			yield return new WaitForSeconds( 3 );
			this.isInvincible = false;
		}

	}

}