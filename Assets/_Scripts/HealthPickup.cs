using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HealthPickup : NetworkBehaviour
{

    [SerializeField] private int healAmount;
    [SerializeField] private float disableTime;
    [SerializeField] private GameObject healthVisual;

    [ServerCallback]
    private void OnTriggerEnter(Collider other) {
        Debug.Log("COlliding with something");
        Health hp = other.gameObject.GetComponent<Health>();
        if (hp) {
            Debug.Log("The something can now be healed");
            hp.ServerHealPlayer(healAmount);
            RpcDisableHealthPack();
            Invoke(nameof(RpcEnableHealthPack), disableTime);
        }
    }

    [ClientRpc]
    public void RpcDisableHealthPack() {
        healthVisual.gameObject.SetActive(false);
    }

    [ClientRpc]
    public void RpcEnableHealthPack() {
        healthVisual.gameObject.SetActive(true);
    }
}
