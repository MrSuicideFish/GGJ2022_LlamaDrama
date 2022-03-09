using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class SettingsManager
{
    private static SettingScreen _screenInst;
    private static AudioMixer _mixer;

    private const string MusicVolumeTag = "MusicVolume";
    private const string SfxVolumeTag = "SfxVolume";
    private const string QualityTag = "MusicVolume";

    public const float MaxGraphicsQuality = 2.0f;

    private static float MusicVolume
    {
        get => PlayerPrefs.GetFloat(MusicVolumeTag, 1.0f);
        set => PlayerPrefs.SetFloat(MusicVolumeTag, value);
    }
    
    private static float SfxVolume
    {
        get => PlayerPrefs.GetFloat(SfxVolumeTag, 1.0f);
        set => PlayerPrefs.SetFloat(SfxVolumeTag, value);
    }

    private static int GraphicsQuality
    {
        get => PlayerPrefs.GetInt(QualityTag, 2);
        set => PlayerPrefs.SetInt(QualityTag, value);
    }

    public static void Initialize()
    {
        ApplyAudioSettings();
    }
    
    public static void OpenSettings()
    {
        if (_screenInst == null)
        {
            SettingScreen scrnRes = Resources.Load<SettingScreen>("UI/SettingScreen");
            if (scrnRes != null)
            {
                _screenInst = SettingScreen.Instantiate(scrnRes);
            }
        }
        
        _screenInst.Show();
    }

    private static void ApplyAudioSettings()
    {
        GetAudioMixer().SetFloat(MusicVolumeTag, MusicVolume);
        GetAudioMixer().SetFloat(SfxVolumeTag, SfxVolume);
    }

    private static AudioMixer GetAudioMixer()
    {
        if (_mixer == null)
        {
            AudioMixer mixerRes = Resources.Load<AudioMixer>("Audio/MainAudioMixer");
            if (mixerRes != null)
            {
                _mixer = mixerRes;
            }
        }
        return _mixer;
    }

    public static void CloseSettings()
    {
        if (_screenInst == null)
        {
            SettingScreen scrnRes = Resources.Load<SettingScreen>("UI/SettingScreen");
            if (scrnRes != null)
            {
                _screenInst = SettingScreen.Instantiate(scrnRes);
            }
        }
        
        _screenInst.Hide();
    }

    public static void SetMusicVolume(float vol)
    {
        MusicVolume = vol;
        ApplyAudioSettings();
    }

    public static float GetMusicVolume()
    {
        return MusicVolume;
    }

    public static void SetSfxVolume(float vol)
    {
        SfxVolume = vol;
        ApplyAudioSettings();
    }

    public static float GetSfxVolume()
    {
        return SfxVolume;
    }

    public static void SetGraphicsQuality(float quality)
    {
        if (quality < 0.0f || quality > 2.0f)
        {
            return;
        }

        GraphicsQuality = (int)quality;
        QualitySettings.SetQualityLevel(GraphicsQuality);
        Debug.Log($"Graphics Quality Set To '{QualitySettings.GetQualityLevel()}'");
    }

    public static int GetGraphicsQuality()
    {
        return GraphicsQuality;
    }
}
