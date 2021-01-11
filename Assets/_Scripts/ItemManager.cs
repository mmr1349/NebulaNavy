﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ItemManager : NetworkBehaviour
{

    public List<Item> items;
    public Item currentItem;
    public int currentItemIndex;

    // Start is called before the first frame update
    /*void Start()
    {
        items = new List<Item>();
        items.AddRange(GetComponentsInChildren<Item>());
        foreach (var it in items) {
            it.gameObject.SetActive(false);
        }
        items[0].gameObject.SetActive(true);
        currentItem = items[0];
        currentItemIndex = 0;
    }*/

    private void Start() {
        Debug.Log("Doing item activation stuff");
        items = new List<Item>();
        items.AddRange(GetComponentsInChildren<Item>(true));
        Debug.Log("Items at start is: " + items);
        currentItem = items[0];
        currentItemIndex = 0;
        for (int i = 0; i < items.Count; i++) {
            items[i].gameObject.SetActive(false);
        }
        items[0].gameObject.SetActive(true);
    }

    public override void OnStartLocalPlayer() {
        //CmdGetAllItems();
    }

    [Command]
    public void CmdActivateNext() {
        Debug.Log("Activating next");
        if (currentItemIndex == items.Count-1) {
            currentItemIndex = 0;
            //currentItem = items[currentItemIndex];
        } else {
            currentItemIndex++;
            //currentItem = items[currentItemIndex];
        }
        RpcUpdateCurrentItem(currentItemIndex);
    }
    
    [Command]
    public void CmdActivatePrevious() {
        Debug.Log("Activating previous");
        if (currentItemIndex == 0) {
            currentItemIndex = items.Count-1;
            //currentItem = items[currentItemIndex];
        }
        else {
            currentItemIndex--;
            //currentItem = items[currentItemIndex];
        }
        RpcUpdateCurrentItem(currentItemIndex);
    }

    [Command]
    public void CmdActivateItem(int index) {
        RpcUpdateCurrentItem(index);
    }

    [ClientRpc]
    private void RpcUpdateCurrentItem(int newIndex) {
        currentItemIndex = newIndex;
        if (currentItem) {
            currentItem.gameObject.SetActive(false);
        }
        currentItem = items[currentItemIndex];
        currentItem.gameObject.SetActive(true);
    }

    [ClientRpc]
    public void RpcGetAllItems() {
        Debug.Log("Trying to get all items");
        items = new List<Item>();
        items.AddRange(GetComponentsInChildren<Item>(true));
        foreach (var it in items) {
            it.gameObject.SetActive(false);
        }
        Debug.Log("Items is now: " + items);
    }

    [Command]
    public void CmdGetAllItems() {
        RpcGetAllItems();
        CmdActivateItem(0);
    }

    [Command]
    public void CmdFireCurrentItem(Vector3 position, Vector3 direction) {
        if (currentItem.useTimer <= 0.0f && currentItem.currentAmmo > 0 && !currentItem.reloading) {
            GameObject bullet = Instantiate(currentItem.projectile, position, Quaternion.identity);
            bullet.transform.forward = direction + (Random.insideUnitSphere * (1-currentItem.accuracyPercent));
            bullet.GetComponent<Projectile>().ServerSetCreator(netId);
            NetworkServer.Spawn(bullet);
            currentItem.useTimer = currentItem.shotInterval;
            RpcModifyAmmoCount(currentItem.currentAmmo - 1);
            if ((Gun)currentItem) {
                RpcRecoilWeapon();
            }
        }
    }

    [ClientRpc]
    public void RpcRecoilWeapon() {
        if ((Gun)currentItem) {
            Gun gunItem = (Gun)currentItem;
            gunItem.OnFire(Time.time);
        }
    }

    [ClientRpc]
    public void RpcRecoverFromRecoil() {
        if ((Gun)currentItem) {
            Gun gunItem = (Gun)currentItem;
            gunItem.RecoverFromRecoil();
            gunItem.ResetRecoilCointer();
        }
    }

    [Command]
    public void CmdReloadItem() {
        currentItem.reloadTimer = currentItem.reloadTime;
        currentItem.reloading = true;
        RpcReloadItem();
    }

    [ClientRpc]
    public void RpcReloadItem() {
        currentItem.PlayReloadAnim();
    }

    [ClientRpc]
    public void RpcModifyAmmoCount(int newCount) {
        currentItem.currentAmmo = newCount;
    }


    private void ItemChanged(int lastIndex, int newIndex) {
        Debug.Log("Item has changed to " + newIndex);
        currentItemIndex = newIndex;
        currentItem.gameObject.SetActive(false);
        currentItem = items[newIndex];
        currentItem.gameObject.SetActive(true);
    }

    [ServerCallback]
    private void Update() {
        currentItem.useTimer -= Time.deltaTime;
        currentItem.reloadTimer -= Time.deltaTime;

        if (currentItem.reloading) {
            if (currentItem.reloadTimer <= 0f) {
                Debug.Log("Is this causing issues?");
                currentItem.reloading = false;
                RpcModifyAmmoCount(currentItem.maxAmmo);
            }
        }
        if ((Gun)currentItem) {
            Gun gunItem = (Gun)currentItem;
            if ((Time.time - gunItem.lastFiredTime) >= gunItem.recoverFromRecoilTime) {
                RpcRecoverFromRecoil();
            }
        }
    }

    public void SetItemLayer(string layerName) {
        foreach (Item it in items) {
            it.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}