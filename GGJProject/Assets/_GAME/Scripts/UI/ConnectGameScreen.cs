using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectGameScreen : MonoBehaviour
{
    public Transform landingPage;
    public Transform connectingPage;

    private void OnEnable()
    {
        ShowLandingPage();
    }

    public void ShowLandingPage()
    {
        landingPage.gameObject.SetActive(true);
        connectingPage.gameObject.SetActive(false);
    }

    public void ShowConnectingPage()
    {
        landingPage.gameObject.SetActive(false);
        connectingPage.gameObject.SetActive(true);
    }
}
