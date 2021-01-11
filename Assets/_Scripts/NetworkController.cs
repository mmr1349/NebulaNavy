using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkController : NetworkBehaviour
{
    private NetworkManager manager;
    [SerializeField] private GameObject bot;

    private void Start() {
        manager = GetComponent<NetworkManager>();
    }


    public override void OnStartServer() {
        base.OnStartServer();
        Debug.Log("Attempting to spawn a bot");
        base.OnStartServer();
        GameObject npc = Instantiate(bot, manager.GetStartPosition().position, Quaternion.identity);
        NetworkServer.Spawn(npc);
    }

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        Debug.Log("Attempting to spawn a bot");
        base.OnStartServer();
        GameObject npc = Instantiate(bot, manager.GetStartPosition().position, Quaternion.identity);
        NetworkServer.Spawn(npc);
    }
}
