using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Random = UnityEngine.Random;
public class NetworkManager : MonoBehaviourPunCallbacks {

    public static ProfileData profileData = new ProfileData();
    public TMP_InputField nicknameField;
    public TMP_InputField ipAddresField;
    public TMP_InputField portField;
    public TMP_InputField roomName;

    private bool nicknameExists = false;

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerPrefs.HasKey("nickname")) {
            nicknameExists = true;
            nicknameField.text = PlayerPrefs.GetString("nickname");
            PhotonNetwork.NickName = nicknameField.text;
        }
    }
    public override void OnConnectedToMaster() {
        PhotonNetwork.LocalPlayer.NickName = nicknameField.text;
        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }
    public void Connect(){
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnJoinedRoom() {
        StartGame();
        base.OnJoinedRoom();
    }

    private void StartGame() {
        //SetNickname();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) PhotonNetwork.LoadLevel(1);
    }

    public void CreateRoom() {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, Photon.Realtime.TypedLobby.Default);
    }

    public void SetNickname() {
        if (nicknameExists) return;
        if (string.IsNullOrEmpty(nicknameField.text)) profileData.nick = "Player " + Random.Range(1, 1000);
        else profileData.nick = nicknameField.text;
        Debug.Log("Nickname: " + profileData.nick);
        PhotonNetwork.NickName = nicknameField.text;

        PlayerPrefs.SetString("nickname", nicknameField.text);
    }

    public static string GenerateUniqueID() {
        var key = "ID";
        var epochStart = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
        var timestamp = (DateTime.UtcNow - epochStart).TotalSeconds;
        var uniqueID = Application.systemLanguage + "-" + Application.platform + "-" + string.Format("{0:X}", Convert.ToInt32(timestamp))
       + "-" + string.Format("{0:X}", Convert.ToInt32(Time.time * 1000000)) + "-" + string.Format("{0:X}", Random.Range(0, 1000000000));
        if (PlayerPrefs.HasKey(key)) {
            uniqueID = PlayerPrefs.GetString(key);
        }
        else {
            PlayerPrefs.SetString(key, uniqueID);
            PlayerPrefs.Save();
        }

        return uniqueID;
    }
}
