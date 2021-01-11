﻿using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour {
    [SerializeField] private GameObject hitMarker;
    [SerializeField] public int damage;
    [SerializeField] private float destroyAfter = 5;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float force = 3000;
    [SerializeField] protected uint creatorNetID;

    public override void OnStartServer() {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    // set velocity for server and client. this way we don't have to sync the
    // position, because both the server and the client simulate it.
    void Start() {
        rigidBody.AddForce(transform.forward * force);
    }

    // destroy for everyone on the server
    [Server]
    void DestroySelf() {
        NetworkServer.Destroy(gameObject);
    }

    [Server]
    public void ServerSetCreator(uint id) {
        creatorNetID = id;
    }

    // ServerCallback because we don't want a warning if OnTriggerEnter is
    // called on the client
    [ServerCallback]
    void OnTriggerEnter(Collider co) {
        ServerOnHitThing(co, co.ClosestPoint(transform.position));
    }
    
    [Server]
    private void ServerOnHitThing(Collider co, Vector3 hitPoint) {
        Debug.Log("Collided with: " + co.name);
        Health hp = co.GetComponent<Health>();
        if (hp) {
            uint hitID = hp.gameObject.GetComponent<NetworkIdentity>().netId;
            if (hitID != creatorNetID) {
                hp.ServerDamageHealthPoints(damage);
            }
            else {
                return;
            }
        }

        SpawnHitMarkers(hitPoint, co.gameObject.transform.forward);
        RpcSpawnHitMarkers(hitPoint, co.gameObject.transform.forward);

        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcSpawnHitMarkers(Vector3 position, Vector3 direction) {
        SpawnHitMarkers(position, direction);
    }

    private void SpawnHitMarkers(Vector3 position, Vector3 direction) {
        Instantiate(hitMarker, position + (direction*0.01f), Quaternion.Euler(direction));
    }

    [ServerCallback]
    private void FixedUpdate() {
        float rayLength = rigidBody.velocity.magnitude*Time.fixedDeltaTime;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength)) {
            ServerOnHitThing(hit.collider, hit.point);
        }
    }
}
