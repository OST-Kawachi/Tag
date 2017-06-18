using UnityEngine;

namespace Common {

	/// <summary>
	/// 共通処理
	/// </summary>
	public class Util {

		/// <summary>
		/// オブジェクトの色を単色で設定する
		/// </summary>
		/// <param name="gameObject">オブジェクト</param>
		/// <param name="color">指定色</param>
		public static void SetColor( GameObject gameObject , Color color ) {

			Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

			foreach( Renderer renderer in renderers ) {
				renderer.material.color = color;
			}

		}


	}

}
