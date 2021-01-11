using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Item : MonoBehaviour
{
    public Transform bulletSpawnLocation;
    public Transform rightHandPosition;
    public Transform leftHandPosition;
    public float shotInterval;
    public GameObject projectile;
    public string itemName;
    public int maxAmmo;
    public int currentAmmo;
    public float reloadTime;
    [Range(0f, 1f)]public float accuracyPercent;


    [HideInInspector] public bool reloading = false;
    [HideInInspector] public float reloadTimer = 0.0f;
    [HideInInspector] public float useTimer = 0.0f;

    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void PlayReloadAnim() {
        if (anim)
            anim.SetTrigger("Reloading");
    }

}
