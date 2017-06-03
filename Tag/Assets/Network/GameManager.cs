using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {

    static public GameManager s_Instance;

    public Text centerText;
    static public List<PlayerInfo> playerList = new List<PlayerInfo>();

    public int EscapePerOni = 5;

    public int playerNum = 0;

    //public class IntList : SyncListStruct<int> { }
    //IntList oniInitList = new IntList();
    public SyncListInt oniInitList = new SyncListInt();

    public void Awake()
    {
        s_Instance = this;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(StartTurm());
    }
	
    IEnumerator StartTurm() {
        yield return new WaitForSeconds(1);
        //OniSelect(OniNumGet(EscapePerOni));
        playerNum = playerList.Count;
        OniInitRandom(OniNumGet(EscapePerOni));
        centerText.text = "鬼決定！";
        yield return new WaitForSeconds(1);
        OniSelect(oniInitList);
 
        centerText.text = "3";
        yield return new WaitForSeconds(1);
        centerText.text = "2";
        yield return new WaitForSeconds(1);
        centerText.text = "1";
        yield return new WaitForSeconds(1);
        centerText.text = "START";
        yield return new WaitForSeconds(1);
        centerText.text = "";
        for (int i = 0; i < playerNum; i++ ) {
            playerList[i].m_Instance.GetComponent<PlayerManagerNetwork>().boolMove = true;
        }
    }

    /*
    void OniSelect(int oniNum) {
        int rand = 0;
        
        for (int i = 0; i < oniNum; i++)
        {
            while (true) {
                rand = Random.Range(0, playerNum);
                if (playerList[rand].m_Instance.GetComponent<PlayerManagerNetwork>().oniSet) { continue; }
                playerList[i].m_Instance.GetComponent<PlayerManagerNetwork>().OniSet();
                break;
            }
        }
    }
    */

    void OniSelect(SyncListInt oniArray)
    {
        foreach (var oni in oniArray)
        {
            playerList[oni].m_Instance.GetComponent<PlayerManagerNetwork>().OniSet();
        }
    }

    int OniNumGet(int num) {
        return playerNum / num + 1;
    }

    static public void AddPlayer(GameObject player, string name, int id)
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.m_Instance = player;
        playerInfo.m_PlayerName = name;
        playerInfo.m_LocalPlayerID = id;

        playerList.Add(playerInfo);
    }

    public void RemovePlayer(GameObject player)
    {
        PlayerInfo toRemove = null;
        foreach (var tmp in playerList)
        {
            if (tmp.m_Instance == player)
            {
                toRemove = tmp;
                break;
            }
        }

        if (toRemove != null)
            playerList.Remove(toRemove);
    }

    static public void PlayerReset() {
        playerList.Clear();
    }

    [Server]
    private void OniInitRandom(int oniNum)
    {
        int rand = 0;
        for (int i = 0; i < oniNum; i++)
        {
            while (true)
            {
                rand = Random.Range(0, playerNum);
                if (playerList[rand].m_Instance.GetComponent<PlayerManagerNetwork>().oniSet) { continue; }
                oniInitList.Add(rand);
                break;
            }
        }
    }
}
