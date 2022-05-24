using UnityEngine;
using UnityEngine.VFX;


public class Gun : Item
{

    [SerializeField] protected VisualEffect muzzleFlash;

    [Header("Recoil Stuff")]
    [SerializeField] public MinMax[] recoilArray;
    [SerializeField] public float recoilRecoveryRate;
    [SerializeField] public float recoilRecoveryDelay;
    [SerializeField] private Transform modelObject;

    [Header("Aiming Stuff")]
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private float aimSpeed;
	public float aimFOV = 45f;

    private Vector3 defaultPosition;
    private Quaternion startRotation;
    private Vector3 forwardAtStart;
		

    private void Start() {
	    startRotation = transform.localRotation;
	    forwardAtStart = transform.forward;
	    defaultPosition = transform.localPosition;
    }

    public virtual void OnFire(float shotTime){
	    Debug.Log("recieved fire rpc");
	    muzzleFlash.Play();
    }

    public void RecoverFromRecoil() {
	    if (!reloading) {
		    transform.localRotation = Quaternion.Slerp(transform.localRotation, startRotation, recoilRecoveryRate * Time.deltaTime);
	    }
    }

    public void Aim() {
	    anim.enabled = false;
	    transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
    }

    public void UnAim() {
	    InvokeRepeating("UnAimInvokee", .1f, .1f);
    }

    private void UnAimInvokee() {
	    anim.enabled = true;
	    transform.localPosition = Vector3.MoveTowards(transform.localPosition, defaultPosition, 2);

	    if (Vector3.Distance(transform.localPosition, defaultPosition) <= 0.05f) {
		    transform.localPosition = defaultPosition;
		    CancelInvoke("UnAimInvokee");
		    return;
	    }
    }
}

