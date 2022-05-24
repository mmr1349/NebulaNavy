using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkController : NetworkBehaviour
{
    public static NetworkController networkController;
    public Dictionary<uint, FpsController> playerDict;
    [SerializeField] private Camera loadOutCam;

    private NetworkManager manager;
    private FpsController localPlayer;

    private void Start() {
        manager = GetComponent<NetworkManager>();
        if (networkController == null) {
            networkController = this;
        }
        playerDict = new Dictionary<uint, FpsController>();
    }

    private void FixedUpdate() {
        if (playerDict.Count < manager.numPlayers) {
            FpsController[] newPlayers = FindObjectsOfType<FpsController>();
            for (int i = 0; i < newPlayers.Length; i++) {
                if (!playerDict.ContainsKey(newPlayers[i].netId)) {
                    playerDict.Add(newPlayers[i].netId, newPlayers[i]);
                    if (newPlayers[i].isLocalPlayer) {
                        localPlayer = newPlayers[i];
                    }
                }
            }
        } else if (playerDict.Count > manager.numPlayers) {
            playerDict.Clear();
            FpsController[] newPlayers = FindObjectsOfType<FpsController>();
            for (int i = 0; i < newPlayers.Length; i++) {
                playerDict.Add(newPlayers[i].netId, newPlayers[i]);
                if (newPlayers[i].isLocalPlayer) {
                    localPlayer = newPlayers[i];
                }
            }
        }
    }


    public override void OnStartServer() {
        base.OnStartServer();
        Debug.Log("Attempting to spawn a bot");
    }

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        Debug.Log("Attempting to spawn a bot");
    }

    [Server]
    public void ServerKillPlayer(uint netID) {
        FpsController player = FindPlayer(netID);
        if (player) {
            player.gameObject.SetActive(false);
            RpcKillPlayer(netID);
        }
    }

    [Command]
    public void CmdRespawnPlayer(uint netID) {
        FpsController player = FindPlayer(netID);
        if (player) {
            player.gameObject.SetActive(true);
            RpcRespawnPlayer(netID);
        }
    }

    public void RespawnLocalPlayer() {
        if (localPlayer) {
            CmdRespawnPlayer(localPlayer.netId);
        }
    }

    [ClientRpc]
    public void RpcRespawnPlayer(uint netID) {
        FpsController player = FindPlayer(netID);
        if (player) {
            player.gameObject.SetActive(true);
            if (player.isLocalPlayer) {
                ClientToggleLoadoutCam(false);
            }
        }
    }

    [ClientRpc]
    public void RpcKillPlayer(uint netID) {
        FpsController player = FindPlayer(netID);
        player.gameObject.SetActive(false);
        if (player.isLocalPlayer) {
            ClientToggleLoadoutCam(true);
        }
    }


    [Client]
    public void ClientToggleLoadoutCam(bool state) {
        if (loadOutCam) {
            loadOutCam.gameObject.SetActive(state);
        } else {
            Debug.LogWarning("Haven't setup load out camera!!!");
        }
    }

    public FpsController FindPlayer(uint netID) {
        FpsController player = playerDict[netID];
        if (player) {
            return player;
        } else {
            return null;
        }
    }

    
}
