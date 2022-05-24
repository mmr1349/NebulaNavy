using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitMarkerParticleController : MonoBehaviour
{

    [SerializeField] private int currentParticleOffset;
    private ParticleSystem partSystem;

    public static HitMarkerParticleController hitMarkerController;
    // Start is called before the first frame update
    void Start()
    {
        if (hitMarkerController == null) {
            hitMarkerController = this;
        } else {
            Destroy(this.gameObject);
        }
        partSystem = GetComponent<ParticleSystem>();
        PlaceParticleAtPositionWithRotation(new Vector3(1, 1, 1), Vector3.zero);
        PlaceParticleAtPositionWithRotation(new Vector3(1, 2, 1), Vector3.zero);
        PlaceParticleAtPositionWithRotation(new Vector3(1, 3, 1), Vector3.zero);
        PlaceParticleAtPositionWithRotation(new Vector3(1, 4, 1), Vector3.zero);
    }

    public void PlaceParticleAtPositionWithRotation(Vector3 position, Vector3 rotation) {
        ParticleSystem.Particle[] hitParticle = new ParticleSystem.Particle[1];
        int particleAlive = partSystem.GetParticles(hitParticle, partSystem.particleCount);
        hitParticle[currentParticleOffset].position = position;
        hitParticle[currentParticleOffset].rotation3D = rotation;
        partSystem.SetParticles(hitParticle);
        currentParticleOffset++;
        if (currentParticleOffset >= particleAlive) {
            currentParticleOffset = 0;
        }
    }
}
