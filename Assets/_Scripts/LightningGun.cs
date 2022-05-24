using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LightningGun : Gun
{
    [SerializeField] private float maxRange = 100f;


    public override void OnFire(float shotTime) {
        Ray ray = new Ray(bulletSpawnLocation.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRange)) {
            muzzleFlash.SetFloat("Length", hit.distance);
        } else {
            muzzleFlash.SetFloat("Length", 0f);
        }
        
        muzzleFlash.Play();
    }
}
