using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartScreen : MonoBehaviour
{
    public ConnectGameScreen JoinGamePanel;
    public string ipConnect = "127.0.0.1";

    private bool isConnectingGame = false;

    private void OnEnable()
    {
        SettingsManager.Initialize();
        JoinGamePanel.gameObject.SetActive(false);
    }

    public void JoinGame()
    {
        NetworkClient.OnConnectedEvent -= OnConnectToGame;
        NetworkClient.OnConnectedEvent += OnConnectToGame;
        
        NetworkClient.OnErrorEvent -= OnErrorConnecting;
        NetworkClient.OnErrorEvent += OnErrorConnecting;
        
        JoinGamePanel.gameObject.SetActive(true);
        JoinGamePanel.ShowLandingPage();
    }

    public void CancelJoinGame()
    {
        if (isConnectingGame)
        {
            isConnectingGame = false;
            JoinGamePanel.ShowLandingPage();
        }
        else
        {
            JoinGamePanel.gameObject.SetActive(false);
        }
    }

    public void SubmitJoinGame()
    {
        isConnectingGame = true;
        JoinGamePanel.ShowConnectingPage();
        Mirror.NetworkClient.Connect(ipConnect);
    }

    private void OnConnectToGame()
    {
        Debug.Log("ON CLIENT CONNECTED!");
    }

    private void OnErrorConnecting(Exception e)
    {
        Debug.Log("ON ERROR CONNECT: " + e.Message);
        JoinGamePanel.ShowLandingPage();
        
#if UNITY_EDITOR
        throw (e);
#endif
    }

    public void SetIpAddress(string ipAddress)
    {
        ipConnect = ipAddress;
    }

    public void HostGame()
    {
        
    }

    public void Settings()
    {
        SettingsManager.OpenSettings();
    }

    private void Update()
    {
        if (isConnectingGame)
        {
            if (!NetworkClient.isConnecting && !NetworkClient.isConnected)
            {
                CancelJoinGame();
            }
        }
    }
}
