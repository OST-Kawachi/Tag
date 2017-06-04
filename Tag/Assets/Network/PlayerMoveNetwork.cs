using System.Collections;
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

    private Text gyroText;

    private Quaternion preGyro;

    // Use this for initialization
    void Start()
    {
        Input.gyro.enabled = true;
        preGyro = Input.gyro.attitude;
        charaCtrl = gameObject.GetComponent<CharacterController>();
        pmn = GetComponent<PlayerManagerNetwork>();
        gyroText = GameObject.Find("Canvas").transform.Find("GyroText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pmn.boolMove && isLocalPlayer) {
            InputKeyMove();
            InputKeyTurn();
            CrossPlatformInputMoveTranslate();
            CrossPlatformTurn();
            GyroTurn();
        }
    }

    void InputKeyMove() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 移動Vector
        Vector3 direction = transform.right * x + transform.forward * z;
        //TranslateMove(direction);
        charaCtrl.Move(direction * speed * 0.1f);
    }

    void CrossPlatformInputMoveTranslate()
    {
        // 右・左
        float x = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        // 上・下
        float z = CrossPlatformInputManager.GetAxisRaw("Vertical");

        // 移動Vector
        Vector3 direction = transform.right * x + transform.forward * z;
        //TranslateMove(direction);
        charaCtrl.Move(direction * speed);
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
        transform.rotation *= Quaternion.AngleAxis(turnSpeed, new Vector3(0, turn, 0));
    }

    void CrossPlatformTurn() {
        float x = CrossPlatformInputManager.GetAxisRaw("HorizontalCam");
        //transform.Rotate(0, x * turnSpeed, 0);
        transform.rotation *= Quaternion.AngleAxis(turnSpeed, new Vector3(0, x, 0));
    }

    void GyroTurn() {
        /* iosはx軸回転が画面右倒ししたときの水平方向?
           wの値もちゃんと考慮が必要そう　値の変化的に　難しい
        */
        float diff = 0;
        Quaternion gyro = Input.gyro.attitude;

        diff = gyro.x - preGyro.x;
        //diff = gyro.y - preGyro.y;
        //diff = gyro.z - preGyro.z;

        gyroText.text = "Gyro"
            + " x:" + gyro.x.ToString("f2") 
            + " y:" + gyro.y.ToString("f2") 
            + " z:" + gyro.z.ToString("f2") 
            + " w:" + gyro.w.ToString("f2");

        if (diff >= 0.005f || diff <= -0.005f) {
            transform.rotation *= Quaternion.AngleAxis(turnSpeed, new Vector3(0, diff, 0));
        }

        preGyro = gyro;
    }
}
