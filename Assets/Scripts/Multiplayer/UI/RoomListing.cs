using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
public class RoomListing : MonoBehaviour {
    public RoomInfo room;
    public TextMeshProUGUI roomNameText;

    public void SetRoomInfo(RoomInfo info) {
        room = info;
        roomNameText.text = info.Name;
    }

    public void Connect() {
        PhotonNetwork.JoinRoom(room.Name);
    }
}
