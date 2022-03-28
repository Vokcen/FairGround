using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

namespace LeaptMultiplayer
{
    public class GameManager : MonoBehaviourPunCallbacks
{
        [SerializeField] private GameObject playerPrefab;



        private void Start()
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region Private Methods
         void LoadArena()
        {  PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity);
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("MasterClient deðilsen yapamazsýn");
            }
            Debug.LogFormat("PhotonNetwork : LoadingLevel : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Game");
          
        }
        #endregion
        #region Photon CallBacks

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
          
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena();

            }
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {

            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}",
                    PhotonNetwork.IsMasterClient);

                LoadArena();
            }


        }
        #endregion
    }
}