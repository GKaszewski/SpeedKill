using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks {
    public Transform content;
    public RoomListing prefab;

    public List<RoomListing> rooms = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        foreach (var info in roomList) {
            if (info.RemovedFromList) {
                int index = rooms.FindIndex(x => x.room.Name == info.Name);
                if (index == -1) return;
                Destroy(rooms[index].gameObject);
                rooms.RemoveAt(index);
            }
            else {
                var listing = Instantiate(prefab, content);
                if (listing != null)
                    listing.SetRoomInfo(info);
            }
           
        }
    }
}
