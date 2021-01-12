﻿using UnityEngine;

namespace Mirror {
	public class Gun : Item
	{
        [SerializeField] private Vector2 recoilModifiers;
        [SerializeField] private Vector2 noiseSampleStart;
        [SerializeField] private float recoilRecoveryRate;
        [SerializeField] public float recoverFromRecoilTime;
        [SerializeField] public float lastFiredTime = 0.0f;

        private Quaternion startRotation;
        private Vector3 forwardAtStart;
        [SerializeField] private int recoilCounter;

        private void Start() {
            startRotation = transform.localRotation;
            forwardAtStart = transform.forward;
        }

        public void OnFire(float shotTime){
			Debug.Log("recieved fire rpc");
            //Apply recoil
            lastFiredTime = shotTime;
            float perlinNoiseX = Mathf.PerlinNoise(noiseSampleStart.x + recoilCounter, noiseSampleStart.y + recoilCounter);
            float perlinNoiseY = Mathf.PerlinNoise(noiseSampleStart.y + recoilCounter, noiseSampleStart.x + recoilCounter);
            transform.forward = transform.forward + (new Vector3(perlinNoiseX * recoilModifiers.x, perlinNoiseY * recoilModifiers.y));
            recoilCounter++;
		}

        public void ResetRecoilCointer() {
            recoilCounter = 0;
        }

        public void RecoverFromRecoil() {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startRotation, recoilRecoveryRate * Time.deltaTime);
            //transform.forward = Vector3.Lerp(tran);
        }
	}
}
