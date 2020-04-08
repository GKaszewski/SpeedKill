using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
public class PlayerListing : MonoBehaviour {
    public Player player;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI deaths;
    public TextMeshProUGUI kills;

    private void Update() {
        if (player == null) return;
        //if (!player.IsLocal) return;
        kills.text =  player.CustomProperties["kills"].ToString();
        deaths.text = player.CustomProperties["deaths"].ToString();
    }
    public void SetPlayer(Player player) {
        this.player = player;
        playerText.text = player.NickName;
    }
}
