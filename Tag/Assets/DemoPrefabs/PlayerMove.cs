using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    private float moveX = 0;
    private float moveZ = 0;
    private float turn = 0;

    private static KeyCode moveRight = KeyCode.A;
    private static KeyCode moveLeft = KeyCode.D;
    private static KeyCode moveForWard = KeyCode.W;
    private static KeyCode moveBack = KeyCode.S;

    private static KeyCode turnRight = KeyCode.RightArrow;
    private static KeyCode turnLeft = KeyCode.LeftArrow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(moveLeft) && !Input.GetKey(moveRight))
        {
            moveX = 0.2f;

        } else if (Input.GetKey(moveRight) && !Input.GetKey(moveLeft))
        {
            moveX = -0.2f;
        } else
        {
            moveX = 0;
        }

        if (Input.GetKey(moveForWard) && !Input.GetKey(moveBack))
        {
            moveZ = 0.2f;
        }
        else if (Input.GetKey(moveBack) && !Input.GetKey(moveForWard))
        {
            moveZ = -0.2f;
        }
        else {
            moveZ = 0;
        }

        transform.Translate(moveX, 0, moveZ, Space.Self);

        if (Input.GetKey(turnRight) && !Input.GetKey(turnLeft))
        {
            turn = 1f;
        }
        else if (Input.GetKey(turnLeft) && !Input.GetKey(turnRight))
        {
            turn = -1;
        }
        else {
            turn = 0;
        }

        transform.Rotate(0, turn, 0);
    }
}
