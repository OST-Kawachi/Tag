﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PlayerMoveNetwork : NetworkBehaviour
{
    public float speed = 0.01f;
    public float turnSpeed = 5.0f;

    private PlayerManagerNetwork pmn;
    private static KeyCode turnRight = KeyCode.J;
    private static KeyCode turnLeft = KeyCode.K;

    private CharacterController charaCtrl;

	public GameObject playerObject;

    // Use this for initialization
    void Start()
    {
        this.charaCtrl = this.gameObject.GetComponent<CharacterController>();
		this.pmn = GetComponent<PlayerManagerNetwork>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( this.pmn.boolMove && this.isLocalPlayer ) {
            InputKeyMove();
            InputKeyTurn();
            CrossPlatformInputMoveTranslate();
        }
    }

    void InputKeyMove() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 移動Vector
        Vector3 direction = this.transform.right * x + this.transform.forward * z;
		//TranslateMove(direction);
		this.charaCtrl.Move(direction * this.speed * 0.1f);
    }

    void CrossPlatformInputMoveTranslate()
    {
        // 右・左
        float x = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        // 上・下
        float z = CrossPlatformInputManager.GetAxisRaw("Vertical");

        // 移動Vector
		Vector3 direction = this.playerObject.transform.right * x + this.playerObject.transform.forward * z;
		//TranslateMove(direction);
		this.charaCtrl.Move(direction * this.speed );
    }

    void InputKeyTurn()
    {
        float turn = 0;

        if (Input.GetKey(turnRight) && !Input.GetKey(turnLeft))
        {
            //turn = 1f;
            turn = -1;
        }
        else if (Input.GetKey(turnLeft) && !Input.GetKey(turnRight))
        {
            //turn = -1;
            turn = 1;
        }
        else
        {
            turn = 0;
        }

		//transform.Rotate(0, turn * turnSpeed, 0);
		this.transform.rotation *= Quaternion.AngleAxis( this.turnSpeed , new Vector3(0, turn, 0));
    }
}
