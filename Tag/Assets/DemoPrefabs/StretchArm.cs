using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchArm : MonoBehaviour {

    private float stretch = 0;
    private GameObject parentArm;
    private bool keyStatus = true;
    private bool stretchStatus = false;

	// Use this for initialization
	void Start () {
        parentArm = transform.root.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        if (stretch > 0.2) {
            transform.position = parentArm.transform.position;
            transform.localScale = parentArm.transform.localScale;
            stretch = 0;
            stretchStatus = false;
            StartCoroutine(CoolTime());
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow) && keyStatus) {
            stretchStatus = true;
            keyStatus = false;
        }

        if (stretchStatus) {
            transform.Translate(0, 0, stretch, Space.Self);
            transform.localScale += new Vector3(0, 0, stretch);
            stretch += 0.02f;
        }
	}

    IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(2);
        keyStatus = true;
    }
}
