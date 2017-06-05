using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroTurn : MonoBehaviour {

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
			turn = new Quaternion (-gyro.x, -gyro.z, -gyro.y, gyro.w) * Quaternion.Euler (90f, 0f, 0f);
			transform.localRotation = turn;
		}
	}
}
