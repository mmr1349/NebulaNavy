using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror {
    public class NetworkSpawner : NetworkBehaviour {
        [Command]
        public void CmdSpawnObjectQuat(GameObject toSpawn, Vector3 location, Quaternion rotation) {
            GameObject spawned = Instantiate(toSpawn, location, rotation);
            NetworkServer.Spawn(spawned);
        }

        [Command]
        public void CmdSpawnObjectDir(GameObject toSpawn, Vector3 location, Vector3 direction) {
            GameObject spawned = Instantiate(toSpawn, location, Quaternion.identity);
            spawned.transform.forward = direction;
            NetworkServer.Spawn(spawned);
        }
    }
}
