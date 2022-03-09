using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScreen : MonoBehaviour
{
    public Slider GraphicsSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;

    public void Awake()
    {
        GraphicsSlider.onValueChanged.AddListener(SettingsManager.SetGraphicsQuality);
        MusicSlider.onValueChanged.AddListener(SettingsManager.SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SettingsManager.SetSfxVolume);
    }

    private void OnEnable()
    {
        GraphicsSlider.value = SettingsManager.GetGraphicsQuality();
        MusicSlider.value = SettingsManager.GetMusicVolume();
        SFXSlider.value = SettingsManager.GetSfxVolume();
        
        Canvas c = this.GetComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceCamera;
        c.worldCamera = Camera.main;
        c.planeDistance = 3.0f;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}