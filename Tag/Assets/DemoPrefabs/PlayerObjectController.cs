using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour {

    public GameObject oniObject;
    public GameObject escapeObject;
    public Color oniColor;
    public Color escapeColor;
    public bool oniSetInit = false;

    private bool invisible;

	// Use this for initialization
	void Start () {
        ColorSet(oniObject, oniColor);
        ColorSet(escapeObject, escapeColor);
        if (oniSetInit)
        {
            OniSet();
        } else{
            EscapeSet();
        }
        invisible = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public bool GetInvisible() {
        return invisible;
    }

    public void InvisibleSet() {
        invisible = true;
        StartCoroutine(CoolTime());
    }

    IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(3);
        invisible = false;
    }

    public void OniSet() {
        oniObject.SetActive(true);
        escapeObject.SetActive(false);
    }

    public void EscapeSet() {
        oniObject.SetActive(false);
        escapeObject.SetActive(true);
    }

    void ColorSet(GameObject gameObject, Color color) {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
}
