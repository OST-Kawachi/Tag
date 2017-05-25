using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeController : MonoBehaviour {

    private GameObject playerObject;
    private PlayerObjectController poc;

	// Use this for initialization
	void Start () {
        playerObject = transform.parent.gameObject;
        poc = playerObject.GetComponent<PlayerObjectController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
