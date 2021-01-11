﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{

    [SyncVar(hook = nameof(HealthChanged))][SerializeField] private int healthPoints;


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
        CmdKillPlayer();
    }

    [Server]
    public void ServerSetHealthPoints(int healthPoints) {
        this.healthPoints = healthPoints;
        ServerKillPlayer();
    }

    [Command]
    public void CmdKillPlayer() {
        if (healthPoints <= 0) {
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
            RpcKillPlayer();
        }
    }

    [Server]
    public void ServerHealPlayer(int amount) {
        healthPoints = Mathf.Clamp(healthPoints+amount, 0, 100);
    }

    [Command]
    public void CmdHealPlater(int amount) {
        ServerHealPlayer(amount);
    }

    private void HealthChanged(int oldHealth, int newHealth) {
        Debug.Log("Health used to be " + oldHealth + " it is now " + newHealth);
    }
}