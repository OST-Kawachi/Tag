using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network.Player {

	//Camera Gyro turn
	public class GyroTurn : MonoBehaviour {

		public GameObject playerObject;

		private Quaternion gyro;
		private Quaternion turn;
		// Use this for initialization
		void Start () {
			Input.gyro.enabled = true;
		}

		// Update is called once per frame
		void Update () {
			if(Input.gyro.enabled){
				gyro = Input.gyro.attitude;
				//iOS 用の回転
				turn = new Quaternion (-gyro.x, -gyro.z, -gyro.y, gyro.w) * Quaternion.Euler (90f, 0f, 0f);
				//Android 用の回転
				//turn = new Quaternion (-gyro.x, gyro.y, -gyro.z, gyro.w) * Quaternion.Euler (90f, 0f, 0f);

				//Camera turn
				transform.localRotation = turn;
				playerObject.transform.rotation = Quaternion.AngleAxis (turn.eulerAngles.y, new Vector3 (0, 1, 0));
			}
		}
	}
}