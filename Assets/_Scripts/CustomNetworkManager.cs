using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class CustomNetworkManager : NetworkManager
{
    public static CustomNetworkManager networkManager;
    public GameObject localPlayer;
    public uint localPlayerNetID;
    public Dictionary<uint, FpsController> players;

    new private void Start() {
        if (networkManager == null) {
            networkManager = this;
        } else {
            Destroy(this.gameObject);
        }
        players = new Dictionary<uint, FpsController>();
    }

    public override void OnStartServer() {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);

        // you can send the message here, or wherever else you want
        CreatePlayerMessage characterMessage = new CreatePlayerMessage {
            name = "Bigger Boss$%^"
        };

        conn.Send(characterMessage);
    }

    public override void OnServerAddPlayer(NetworkConnection conn) {
        base.OnServerAddPlayer(conn);
    }

    void OnCreateCharacter(NetworkConnection conn, CreatePlayerMessage message) {
        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject gameobject = Instantiate(playerPrefab);
        gameobject.transform.position = GetStartPosition().position;

        // Apply data from the message however appropriate for your game
        // Typically Player would be a component you write with syncvars or properties
       
        FpsController player = gameobject.GetComponent<FpsController>();
        player.SetName(message.name);
        
        //players.Add(player.netId, player);

        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, gameobject);
        players.Add(player.netId, player);
    }

    public override void OnServerReady(NetworkConnection conn) {
        base.OnServerReady(conn);  

        
    }
}

public struct CreatePlayerMessage : NetworkMessage {
    public string name;
}
