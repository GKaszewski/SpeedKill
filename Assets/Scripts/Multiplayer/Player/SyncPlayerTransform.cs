using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SyncPlayerTransform : MonoBehaviourPunCallbacks {
    private CharacterController characterController;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(characterController.transform.position);
            stream.SendNext(characterController.transform.rotation);
            stream.SendNext(characterController.velocity);
        }
        else {
            characterController.transform.position = (Vector3)stream.ReceiveNext();
            characterController.transform.rotation = (Quaternion)stream.ReceiveNext();
            characterController.SimpleMove((Vector3)stream.ReceiveNext());

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTimestamp));
            characterController.transform.position += characterController.velocity * lag;
        }
    }
}
