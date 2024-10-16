using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string playerPrefabPath;
    public Transform[] spawnPoints;
    public float respawnTime;
    private int playersInGame;
    // instance
    public static GameManager instance;
    void Awake ()
    {
        instance = this;
    }

    void SpawnPlayer ()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        // initialize the player
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
