using System.Net;
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

		/// <summary>
		/// 自身のIPV4アドレスを取得する
		/// </summary>
		/// <returns></returns>
		public static string GetMyIpv4() {

			string ipv4 = null;

			IPHostEntry ipHostEntry = Dns.GetHostEntry( Dns.GetHostName() );
			foreach( IPAddress ipAddress in ipHostEntry.AddressList ) {
				// IPV4のアドレス
				if( ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ) {
					ipv4 = ipAddress.ToString();
				}
			}

			if( ipv4 == null )
				Debug.LogWarning( "IPV4アドレスが取得できませんでした" );

			return ipv4;

		}


	}

}
