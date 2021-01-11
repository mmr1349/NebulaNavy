using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NetworkTransform))]
public class FpsController : NetworkBehaviour {

    [Header("Toggled Components")]
    [SerializeField] ToggleEvent OnToggleShared;
    [SerializeField] ToggleEvent OnToggleLocal;
    [SerializeField] ToggleEvent OnToggleRemote;

    [Header("Attributes")]
    [SerializeField] Gun gun;
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    NetworkSpawner spawner;
    Health health;
    ItemManager itemManager;

    [HideInInspector]
    public bool canMove = true;

    private PlayerUIController uiController;
    [SyncVar] public float horizontal;
    [SyncVar] public float vertical;
    [SyncVar] public bool jumping;

    void Start() {
        EnablePlayer();
        if (isLocalPlayer) {
            gameObject.name = "LocalPlayer";
        } else {
            gameObject.name = "EnemyPlayer";
        }
        //if (!isLocalPlayer) return;
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCamera = GetComponentInChildren<Camera>();
        spawner = GetComponent<NetworkSpawner>();
        health = GetComponent<Health>();
        itemManager = GetComponent<ItemManager>();

        if (isLocalPlayer) {
            uiController = FindObjectOfType<PlayerUIController>();
            itemManager.SetItemLayer("InHand");
            gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
        } else {
            Debug.Log("Setting item layer to default");
            itemManager.SetItemLayer("Default");
        }
    }

    private void EnablePlayer() {
        OnToggleShared.Invoke(true);
        if (isLocalPlayer) {
            OnToggleLocal.Invoke(true);
            OnToggleRemote.Invoke(false);
        } else {
            OnToggleLocal.Invoke(false);
            OnToggleRemote.Invoke(true);
        }
    }

    void Update() {
        //Return if we aren't the player.
        if (!isLocalPlayer || playerCamera == null) return;
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        jumping = Input.GetButton("Jump") && canMove && characterController.isGrounded;
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * vertical : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * horizontal : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded) {
            moveDirection.y = jumpSpeed;
        }
        else {
            moveDirection.y = movementDirectionY;
        }

        

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove) {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (Input.GetButtonDown("Fire1")) {
            //gun.Fire();
            Debug.Log("Calling spawn stuff");
            //CmdSpawnStuff(itemManager.currentItem.bulletSpawnLocation.position, itemManager.currentItem.bulletSpawnLocation.forward);
            itemManager.CmdFireCurrentItem(itemManager.currentItem.bulletSpawnLocation.position, itemManager.currentItem.bulletSpawnLocation.forward);
        }

        if (Input.GetButtonDown("Reload")) {
            itemManager.CmdReloadItem();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = Cursor.visible ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
        }

        if (Input.GetAxis("Mouse ScrollWheel") >= 0.01f) {
            itemManager.CmdActivateNext();
            Debug.Log("Scrolling up");
        } else if (Input.GetAxis("Mouse ScrollWheel") <= -0.01f) {
            itemManager.CmdActivatePrevious();
            Debug.Log("Scrolling down");
        }

        uiController.SetHealthSliderValue(health.GetHealthPoints());
        uiController.SetWeaponNameText(itemManager.currentItem.name);
        uiController.SetWeaponAmmoText(itemManager.currentItem.maxAmmo, itemManager.currentItem.currentAmmo);
    }

    [Command]
    private void CmdSpawnStuff(Vector3 spawnLocation, Vector3 direction) {
        Debug.Log("Trying to spawn object");
        if (!itemManager.currentItem) {
            itemManager.CmdGetAllItems();
        }
        GameObject bullet = Instantiate(itemManager.currentItem.projectile, spawnLocation, Quaternion.identity);
        bullet.transform.forward = direction;
        NetworkServer.Spawn(bullet);
    }

    /*[ServerCallback]
    private void OnCollisionEnter(Collision collision) {
        Projectile proj = collision.gameObject.GetComponent<Projectile>();
        Debug.Log("Collided with something");
        if (proj) {
            Debug.Log("Collided with bullet damaging ourselves");
            health.ServerDamageHealthPoints(proj.damage);
        }
    }*/

    [ServerCallback]
    private void OnDisable() {
        //When we are disabled start a timer to undisable
        Invoke("ServerUndisablePlayer", 10f);
    }

    [Server]
    void ServerUndisablePlayer() {
        health.ServerSetHealthPoints(100);
        RpcUndisablePlayer();
    }

    [ClientRpc]
    void RpcUndisablePlayer() {
        gameObject.SetActive(true);
    }
}
