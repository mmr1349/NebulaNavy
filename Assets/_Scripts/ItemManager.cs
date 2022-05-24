using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ItemManager : NetworkBehaviour
{

    public List<Item> items;
    public Item currentItem;
    public int currentItemIndex;

    private FpsController player;

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
        player = GetComponent<FpsController>();
    }

    public override void OnStartLocalPlayer() {
        //CmdGetAllItems();
    }

    [Command]
    public void CmdActivateNext() {
        Debug.Log("Activating next");
        player.ResetRecoilData();
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
        player.ResetRecoilData();
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
        player.ResetRecoilData();
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
            /*float perlinNoiseX = Mathf.PerlinNoise(gunItem.noiseSampleStart.x + gunItem.recoilCounter, gunItem.noiseSampleStart.y + gunItem.recoilCounter);
            float perlinNoiseY = Mathf.PerlinNoise(gunItem.noiseSampleStart.y + gunItem.recoilCounter, gunItem.noiseSampleStart.x + gunItem.recoilCounter);*/
            /*float movementX = Random.Range(gunItem.minRecoil.x, gunItem.maxRecoil.x);
            float movementY = Random.Range(gunItem.minRecoil.y, gunItem.maxRecoil.y);*/
            //Vector3 camRecoil = new Vector3(movementX, movementY, 0f);
            //player.Recoil(movementX, gunItem.recoilRecoveryRate, gunItem.recoilModifiers.x);
            player.Recoil(gunItem.recoilArray, gunItem.recoilRecoveryRate, gunItem.recoilRecoveryDelay);
        }
    }

    [ClientRpc]
    public void RpcRecoverFromRecoil() {
        
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
    }

    public void AimWeapon() {
        if ((Gun)currentItem) {
            Gun gunItem = (Gun)currentItem;
            Debug.Log("Aiming the gun");
            gunItem.Aim();
        }
    }

    public void UnAimWeapon() {
        if ((Gun)currentItem) {
            Gun gunItem = (Gun)currentItem;
            gunItem.UnAim();
        }
    }

    public void SetItemLayer(string layerName) {
        foreach (Item it in items) {
            it.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    public float GetAimFOV() {
        if ((Gun)currentItem) {
            return ((Gun)currentItem).aimFOV;
        }
        return 50f;
    }

    public void SetItems(GameObject item1, GameObject item2) {
        Transform parent = items[0].transform.parent;
        for (int i = 0; i < items.Count; i++) {
            Destroy(items[i].gameObject);
        }
        items.Clear();

        items.Add(Instantiate(item1, parent).GetComponent<Item>());
        items.Add(Instantiate(item2, parent).GetComponent<Item>());
        items[1].gameObject.SetActive(false);
        currentItem = items[0];
    }
}
