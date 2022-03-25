using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
namespace LeaptMultiplayer
{
    public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";

    [Tooltip("ODaya Katýlabilcek maksimum kullanýcý sayýsý")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;

        public static Action OnAttempToConnect,OnDisconnect;

    #region UnityEvents

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
  


    #endregion


    #region Public

    public void Connect()
    {
            OnAttempToConnect?.Invoke();
            isConnecting = true;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
                PhotonNetwork.ConnectUsingSettings();

                PhotonNetwork.GameVersion = gameVersion;
        }
    }
    #endregion


    #region PunCallBacks

    public override void OnDisconnected(DisconnectCause cause)
    {
            OnDisconnect?.Invoke();
            isConnecting = false;
        Debug.Log("<color=red>Çýkýþ Yapýldý</color>");
    }
    public override void OnConnectedToMaster()
    {

        Debug.Log("<color=green>Master'a Baðlanýldý</color>");
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
    }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Odaya katýlma baþarýsýz oldu Rastgele oda yok.Yeni oda oluþturuldu");
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom });

        }
  
    public override void OnJoinedRoom()
    { //#Critical : We only load if we are the first player, else we rely on 'photonnnetwork.autimatcasylysceneour instanse,
        Debug.Log("Odaya Katýlýndý");
            if (PhotonNetwork.CurrentRoom.PlayerCount==1)
            {
                Debug.Log("Oda 1 açýldý");

                //#Kritik
                //Load the Room Level
                PhotonNetwork.LoadLevel("Room for 1");
            }
    }

    #endregion
}
}
