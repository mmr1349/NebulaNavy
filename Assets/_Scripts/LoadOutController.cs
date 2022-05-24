using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;


public class LoadOutController : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI[] selectedItemDisplays = new TextMeshProUGUI[2];
    [SerializeField] private Button[] selectedItemButtons = new Button[2];

    [SerializeField] private int selectedIndex = 0;
    [SerializeField] private GameObject[] selectedItems = new GameObject[2];
    [SerializeField] private GameObject[] items;

    public struct Loadout : NetworkMessage {
        public int item1Index;
        public int item2Index;
        public bool armor;
        public bool spaceSuit;
    }

    public void SendLoadout(int index1, int index2, bool armor, bool spaceSuit) {
        Loadout msg = new Loadout() {
            item1Index = index1,
            item2Index = index2,
            armor = armor,
            spaceSuit = spaceSuit
        };

        NetworkServer.SendToAll(msg);
    }

    [Server]
    public void ServerGainAuthority(NetworkConnection playerConnection) {
        if (netIdentity.hasAuthority) {
            netIdentity.RemoveClientAuthority();
        }
        netIdentity.AssignClientAuthority(playerConnection);
    }

    public void SendRespawnCommand() {
        Debug.Log("Sending respawn command to server");
        CmdRespawnPlayer(CustomNetworkManager.networkManager.localPlayerNetID);
        gameObject.SetActive(false);
    }


    [Command(ignoreAuthority = true)]
    public void CmdRespawnPlayer(uint netID) {
        GameObject player = CustomNetworkManager.networkManager.players[netID].gameObject;
        player.GetComponent<ItemManager>().SetItems(selectedItems[0], selectedItems[1]);
        player.SetActive(true);
        player.GetComponent<Health>().ServerSetHealthPoints(100);
        RpcRespawnPlayer();
    }

    [ClientRpc]
    private void RpcRespawnPlayer() {
        CustomNetworkManager.networkManager.localPlayer.GetComponent<ItemManager>().SetItems(selectedItems[0], selectedItems[1]);
        CustomNetworkManager.networkManager.localPlayer.SetActive(true);
        if (isLocalPlayer) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    /*public bool ValidateWeapons() {
        for (int i = 0; i < selectedItems.Length; i++) {
        }
    }*/


    public void SelectItemToModify(int index) {
        Debug.Log("Seleted item: " + index);
        CmdSelectItemToModify(index);
    }

    [Command(ignoreAuthority = true)]
    private void CmdSelectItemToModify(int index) {
        selectedIndex = index;
        RpcSelectItemToModify(index);
    }

    [ClientRpc]
    private void RpcSelectItemToModify(int index) {
        selectedIndex = index;
    }

    public void SelectItem(int index) {
        Debug.Log("Selected an item to equip");
        CmdSelectItem(index);
    }

    [Command(ignoreAuthority = true)]
    private void CmdSelectItem(int index) {
        selectedItems[selectedIndex] = items[index];
        selectedItemDisplays[selectedIndex].text = items[index].name;
        RpcSelectItems(index);
    }

    [ClientRpc]
    private void RpcSelectItems(int index) {
        selectedItems[selectedIndex] = items[index];
        selectedItemDisplays[selectedIndex].text = items[index].name;
    }

    public void ChangeButtonColor(int index) {
        for (int i = 0; i < selectedItemButtons.Length; i++) {
            if (i == index) {
                selectedItemButtons[index].image.color = selectedItemButtons[index].colors.selectedColor;
            } else {
                selectedItemButtons[i].image.color = selectedItemButtons[i].colors.normalColor;
            }
        }
    }
}
