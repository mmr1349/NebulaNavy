using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{

    [SerializeField] private LoadOutController loadOutScreen;
    [SyncVar(hook = nameof(HealthChanged))][SerializeField] private int healthPoints;


    private void Start() {
        loadOutScreen = FindObjectOfType<LoadOutController>(true);
    }

    public int GetHealthPoints() {
        return healthPoints;
    }

    [Command]
    public void CmdDamageHealthPoints(int damage) {
        healthPoints -= damage;
        CmdKillPlayer();
    }

    [Command]
    public void CmdSetHealthPoints(int healthPoints) {
        this.healthPoints = healthPoints;
        ServerKillPlayer();
    }

    [Server]
    public void ServerSetHealthPoints(int healthPoints) {
        this.healthPoints = healthPoints;
        ServerKillPlayer();
    }

    [Command]
    public void CmdKillPlayer() {
        if (healthPoints <= 0) {
            //NetworkController.networkController.ServerKillPlayer(this.netId);
            RpcKillPlayer();
        }
    }

    [ClientRpc]
    public void RpcKillPlayer() {
        Debug.Log("Killing has been called");
        this.gameObject.SetActive(false);
    }

    [Server]
    public void ServerDamageHealthPoints(int damage) {
        healthPoints -= damage;
        ServerKillPlayer();
    }

    [Server]
    public void ServerKillPlayer() {
        if (healthPoints <= 0) {
            //NetworkController.networkController.ServerKillPlayer(this.netId);
            //loadOutScreen.ServerGainAuthority(netIdentity.connectionToClient);
            //TargetOpenLoadoutScreen(netIdentity.connectionToClient);
            RpcKillPlayer();
            GameController.gameController.TargetSetRespawnCounter(netIdentity.connectionToClient);
            Invoke("ServerRespawnPlayer", GameController.gameController.respawnTime);
        }
    }

    [TargetRpc]
    private void TargetOpenLoadoutScreen(NetworkConnection target) {
        Debug.Log("we have been told to die and open up the loadout screen");
        loadOutScreen.gameObject.SetActive(true);
    }

    [Server]
    public void ServerHealPlayer(int amount) {
        healthPoints = Mathf.Clamp(healthPoints+amount, 0, 100);
    }

    [Command]
    public void CmdHealPlater(int amount) {
        ServerHealPlayer(amount);
    }

    [Server]
    private void ServerRespawnPlayer()
    {
        ServerSetHealthPoints(100);
        GameController.gameController.ServerSetPlayerSpawnPoint(this.transform);
        RpcRespawnPlayer();
    }

    [ClientRpc]
    private void RpcRespawnPlayer()
    {
        this.gameObject.SetActive(true);
    }

    private void HealthChanged(int oldHealth, int newHealth) {
        Debug.Log("Health used to be " + oldHealth + " it is now " + newHealth);
    }
}
