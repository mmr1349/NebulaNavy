using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public struct playerInfo
{
    public string username;
    public bool canSpawn;
    public GameObject player;
    public int kills;
    public int deaths;
    public int points;
}


public class GameController : NetworkBehaviour
{
    public static GameController gameController;
    [SerializeField] private Dictionary<string, playerInfo> playerInfoDict;
    [SerializeField] public int respawnTime;
    [SerializeField] private RespawnCanvas respawnCanvas;
    [SerializeField] private NetworkStartPosition[] respawnPoints;

    private void Start()
    {
        if (gameController != null)
        {
            Destroy(this.gameObject);
        }
        gameController = this;
        respawnPoints = FindObjectsOfType<NetworkStartPosition>();
        playerInfoDict = new Dictionary<string, playerInfo>();
    }

    [Server]
    public void ServerSetPlayerSpawnPoint(Transform player)
    {
        var index = Random.Range(0, respawnPoints.Length - 1);
        player.position = respawnPoints[index].transform.position;
        RpcSetPlayerSpawnPoint(player, index);
    }

    [TargetRpc]
    public void TargetSetRespawnCounter(NetworkConnection identity) 
    {
        respawnCanvas.StartCountdown(respawnTime);
    }

    [ClientRpc]
    private void RpcSetPlayerSpawnPoint(Transform player, int index)
    {
        player.position = respawnPoints[index].transform.position;
    }

    [Server]
    public void ServerSendLeaderboardData() {
        RpcGetLeaderboardData(playerInfoDict.Values.ToList());
    }

    [Server]
    public void AddKillUsername(string username) {
        playerInfo info;
        if (playerInfoDict.TryGetValue(username, out info)) {
            info.kills += 1;
            info.points += 100;
            playerInfoDict.Add(username, info);
        }
    }

    [ClientRpc]
    public void RpcGetLeaderboardData(List<playerInfo> playersInfo) {
        for (int i = 0; i < playersInfo.Count; i++) {
            Debug.Log(playersInfo[i].username + " has " + playersInfo[i].kills + " kills and " + playersInfo[i].points + " points");
        }
    }
}
