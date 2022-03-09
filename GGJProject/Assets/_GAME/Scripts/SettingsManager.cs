using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsManager
{
    private static SettingScreen screenInst;

    private static float GameVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("GameVolume", 1.0f);
        }

        set
        {
            PlayerPrefs.SetFloat("GameVolume", value);
        }
    }

    public static void OpenSettings()
    {
        if (screenInst == null)
        {
            SettingScreen scrnRes = Resources.Load<SettingScreen>("UI/SettingScreen");
            if (scrnRes != null)
            {
                screenInst = SettingScreen.Instantiate(scrnRes);
            }
        }
        
        screenInst.Show();
    }

    public static void CloseSettings()
    {
        if (screenInst == null)
        {
            SettingScreen scrnRes = Resources.Load<SettingScreen>("UI/SettingScreen");
            if (scrnRes != null)
            {
                screenInst = SettingScreen.Instantiate(scrnRes);
            }
        }
        
        screenInst.Hide();
    }

    public static void SetVolume(float vol)
    {
        GameVolume = vol;
    }

    public static float GetVolume()
    {
        return GameVolume;
    }
}
