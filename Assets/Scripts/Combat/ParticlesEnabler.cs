using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesEnabler : MonoBehaviour {
    public ParticleSystem[] particles;

    private void OnDisable() {
        foreach (var particle in particles)
            particle.Stop();
    }

    private void OnEnable() {
        foreach (var particle in particles)
            particle.Play();
    }
}
