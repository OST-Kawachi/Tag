using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniController : MonoBehaviour {

    private GameObject playerObject;
    private GameObject escapeObject;
    public PlayerObjectController poc;
    private PlayerObjectController escpoc;

    public int count = 0;

    // Use this for initialization
    void Start()
    {
        playerObject = transform.parent.gameObject;
        poc = playerObject.GetComponent<PlayerObjectController>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Escape")
        {
            escapeObject = other.gameObject.transform.parent.gameObject;
            escpoc = escapeObject.GetComponent<PlayerObjectController>();

            if (!escpoc.GetInvisible()) {
                poc.EscapeSet();
                poc.InvisibleSet();
                escpoc.OniSet();
            }
        }
    }
}
