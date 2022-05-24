using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameModeController : NetworkBehaviour
{
    private List<FpsController> players;

    private void Start() {
        players = new List<FpsController>();
    }

    /*public override void OnServerAddPlayer() {
        players = new List<FpsController>(FindObjectsOfType<FpsController>());
    }*/
}
