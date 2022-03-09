using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private const string PauseScreenTemplatePath = "UI/PauseScreen";
    
    private static PauseScreen pauseUIScreen;
    public static bool IsPaused { get; private set; }

    public static bool ShouldPause
    {
        get
        {
            if (GameManager.Instance != null &&
                (GameManager.Instance.gameHasStarted || GameManager.Instance.gameHasEnded))
            {
                return true;
            }
            
            return false;
        }
    }

    public void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
        
        // load pause screen instance
        if (pauseUIScreen == null)
        {
            PauseScreen pauseScreenResource = Resources.Load<PauseScreen>(PauseScreenTemplatePath);
            if (pauseScreenResource != null)
            {
                pauseUIScreen = PauseScreen.Instantiate(pauseScreenResource);
                pauseUIScreen.Hide();
            }
        }
    }

    public static void Resume()
    {
        pauseUIScreen.Hide();
        IsPaused = false;
    }

    public static void Pause()
    {
        pauseUIScreen.Show();
        IsPaused = true;
    }
}
