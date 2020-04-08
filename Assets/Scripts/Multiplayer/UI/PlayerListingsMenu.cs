using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks {
    public Transform content;
    public PlayerListing prefab;

    public List<PlayerListing> players = new List<PlayerListing>();

    private void Awake() {
        GetCurrentRoomPlayers();
    }

    private void Update() {
         
    }

    private void GetCurrentRoomPlayers() {
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values) {
            AddPlayerToTheListing(player);
        }
    }

    private void AddPlayerToTheListing(Player player) {
        var newListing = Instantiate(prefab, content);
        if (newListing != null) {
            newListing.SetPlayer(player);
            players.Add(newListing);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.LogError($"{newPlayer.NickName} joined the room!");
        AddPlayerToTheListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.LogError($"{otherPlayer.NickName} left the room!");
        int index = players.FindIndex(x => x.player == otherPlayer);
        if (index == -1) return;
        Destroy(players[index].gameObject);
        players.RemoveAt(index);
    }
}
