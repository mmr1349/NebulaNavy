using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShotgunShell : Projectile
{
    [SerializeField] private GameObject shot;
    [SerializeField] private int shotCount;
    [SerializeField][Range(0f,1f)] private float randomNessModifier;


    // Start is called before the first frame update
    void Start()
    {
        if (isServer) {
            ServerSpawnShot();
        }
        Destroy(this.gameObject);
    }

    [Server]
    private void ServerSpawnShot() {
        for (int i = 0; i < shotCount; i++) {
            GameObject shotInstance = Instantiate(shot, transform.position, Quaternion.identity);
            shotInstance.transform.forward = transform.forward + (Random.insideUnitSphere * randomNessModifier);
            shotInstance.GetComponent<Projectile>().ServerSetCreator(creatorNetID);
            NetworkServer.Spawn(shotInstance);
        }
    }
}
