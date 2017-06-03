using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManagerNetwork : NetworkBehaviour
{
    public GameObject oniObject;
    public GameObject escapeObject;
    public Color oniColor;
    public Color escapeColor;

    [SyncVar]
    public bool oniSet = false;
    public int playerUniqueNumber = 0;
    [SyncVar]
    public bool boolMove = false;

    public void Awake()
    {
        GameManager.AddPlayer(gameObject, "player", 1);
    }

    public void Start()
    {
        InitColorSet();
        ObjectSet();
        if (!isLocalPlayer)
        {
            gameObject.transform.Find("PlayerCamera").gameObject.SetActive(false);
        }

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.gameObject.name);
        if (gameObject.tag == "Oni")
        {
            if (hit.gameObject.tag == "Escape")
            {
                //EscapeSet();
                //hit.gameObject.GetComponent<PlayerManagerNetwork>().OniSet();
                RpcOniTouch(hit.gameObject);
            }
        }
    }

    [ClientRpc]
    public void RpcOniTouch(GameObject escape) {
        EscapeSet();
        escape.GetComponent<PlayerManagerNetwork>().OniSet();
    }

    public void OniSet()
    {
        oniSet = true;
        oniObject.SetActive(true);
        escapeObject.SetActive(false);
        gameObject.tag = "Oni";
    }

    public void EscapeSet()
    {
        oniSet = false;
        oniObject.SetActive(false);
        escapeObject.SetActive(true);
        gameObject.tag = "Escape";
    }

    void ColorSet(GameObject gameObject, Color color)
    {
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }

    void InitColorSet()
    {
        ColorSet(oniObject, oniColor);
        ColorSet(escapeObject, escapeColor);
    }

    void ObjectSet()
    {
        if (oniSet) {
            OniSet();
        } else {
            EscapeSet();
        }
    }


    public override void OnNetworkDestroy()
    {
        Debug.Log("destroy" + gameObject.name);
        GameManager.s_Instance.RemovePlayer(gameObject);
    }
}
