using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using LeaptMultiplayer;
using System;

public class LoginCanvas : MonoBehaviour
{
    [SerializeField] GameObject loginCanvas, loadingCanvas;


    private void Awake()
    {
        ActiveLogin();
        Launcher.OnAttempToConnect += ActivateLoading;
        Launcher.OnDisconnect += ActiveLogin;
    }
    private void OnDestroy()
    {
        Launcher.OnAttempToConnect -= ActiveLogin;
        Launcher.OnDisconnect -= ActivateLoading;

    }

    private void ActivateLoading()
    {
        loginCanvas.gameObject.SetActive(false);
        loadingCanvas.gameObject.SetActive(true);
    }

    private void ActiveLogin()
    {
        loginCanvas.gameObject.SetActive(true);
        loadingCanvas.gameObject.SetActive(false);
    }
    
}
