using UnityEngine;

namespace Mirror {
	public class Gun : Item
	{

        [Header("Recoil Stuff")]
        [SerializeField] public Vector2 recoilModifiers;
        [SerializeField] public Vector2 noiseSampleStart;
        [SerializeField] public float recoilRecoveryRate;
        [SerializeField] public float recoverFromRecoilTime;
        [SerializeField] public float lastFiredTime = 0.0f;
        [SerializeField] public int recoilCounter;

        public Vector2 minRecoil;
        public Vector2 maxRecoil;


        [Header("Aiming Stuff")]
        [SerializeField] private Vector3 aimPosition;
        [SerializeField] private float aimSpeed;

        private Vector3 defaultPosition;
        private Quaternion startRotation;
        private Vector3 forwardAtStart;
        

        private void Start() {
            startRotation = transform.localRotation;
            forwardAtStart = transform.forward;
            defaultPosition = transform.localPosition;
        }

        public void OnFire(float shotTime){
			Debug.Log("recieved fire rpc");
            //Apply recoil
            lastFiredTime = shotTime;
            /*float perlinNoiseX = Mathf.PerlinNoise(noiseSampleStart.x + recoilCounter, noiseSampleStart.y + recoilCounter);
            float perlinNoiseY = Mathf.PerlinNoise(noiseSampleStart.y + recoilCounter, noiseSampleStart.x + recoilCounter);
            //transform.forward = transform.forward + (new Vector3(perlinNoiseX * recoilModifiers.x, perlinNoiseY * recoilModifiers.y));
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x + perlinNoiseX * recoilModifiers.x, transform.localRotation.eulerAngles.y + perlinNoiseY * recoilModifiers.y, transform.localRotation.eulerAngles.z);
            recoilCounter++;*/
		}

        public void ResetRecoilCointer() {
            recoilCounter = 0;
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
}
