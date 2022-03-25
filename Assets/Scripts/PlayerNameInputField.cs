using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

namespace LeaptMultiplayer { 
public class PlayerNameInputField : MonoBehaviour
{

    const string _playerNameKey = "PlayerName";


    private void Start()
    {
        string defaultName = string.Empty;
            TMP_InputField playerNameInput = GetComponent<TMP_InputField>();
        if (playerNameInput!=null)
        {
            if (PlayerPrefs.HasKey(_playerNameKey))
            {
                defaultName = PlayerPrefs.GetString(_playerNameKey);
                playerNameInput.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        //#Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Kullanýcý adý boþ olmamalý");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(_playerNameKey, value);
    }

}
}
