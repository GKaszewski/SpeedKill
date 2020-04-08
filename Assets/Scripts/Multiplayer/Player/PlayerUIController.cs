using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class PlayerUIController : MonoBehaviourPunCallbacks {
    private bool displayInfoTab = false;
    public TextMeshProUGUI nickname;
    public GameObject infoTab;

    public Behaviour[] controllerComponents;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        if (photonView.IsMine) nickname.text = PhotonNetwork.LocalPlayer.NickName;
        infoTab.SetActive(false);
    }

    private void Update() {
        if (!photonView.IsMine) return;

        infoTab.SetActive(displayInfoTab);
     
        if(Input.GetKeyDown(KeyCode.Tab)) {
            if (displayInfoTab) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                foreach (var component in controllerComponents)
                    component.enabled = true;
                displayInfoTab = false;
            }
            else {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                foreach (var component in controllerComponents)
                    component.enabled = false;
                displayInfoTab = true;
            }
        }
    }


    public void Suicide() {
        NetworkGameManager.instance.AddDeaths(photonView.Owner);
        photonView.RPC("RpcKill", RpcTarget.All);
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LoadLevel(0);
    }
}
