using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AnimateGun : MonoBehaviourPunCallbacks {
    public GameObject gun;
    [PunRPC]
    public void AnimateThirdPersonGun(float reloadTime) {
        //if (photonView.IsMine)
            LeanTween.rotateAroundLocal(gun, Vector3.right, 360f, reloadTime - 0.15f).setEase(LeanTweenType.easeSpring);
    }
}
