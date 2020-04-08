using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class JetpackParticles : MonoBehaviourPunCallbacks {
    public ParticleSystem[] fires;

    private void Start() {
        foreach (var item in fires) {
            var emission = item.emission;
            emission.enabled = false;
        }
    }

    [PunRPC]
    public void TurnOnParticles() {
        foreach (var item in fires) {
            var emission = item.emission;
            emission.enabled = true;
        }
    }
    [PunRPC]
    public void TurnOffParticles() {
        foreach (var item in fires) {
            var emission = item.emission;
            emission.enabled = false;
        }
    }
}
