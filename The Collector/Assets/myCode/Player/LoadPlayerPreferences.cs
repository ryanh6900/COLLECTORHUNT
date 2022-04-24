using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadPlayerPreferences : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Audio Settings")]
    [SerializeField] private TMP_Text volumeTextVal = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextVal = null;
    
    [Header("Quality Level Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    
    [Header("Fullscreen Settings")]
    [SerializeField] private Toggle fullScreenToggle = null;

    [Header("Sensitivity Settings")]
    [SerializeField] private Slider mouseXSenSlider = null;
    [SerializeField] private TMP_Text mouseXSensValue = null;
    [SerializeField] private Slider mouseYSensSlider = null;
    [SerializeField] private TMP_Text mouseYSensValue = null;

    [Header("Invert Y Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");
                volumeTextVal.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else menuController.ResetBtn("Audio");

            if (PlayerPrefs.HasKey("masterOverallQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterOverallQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }
            if (PlayerPrefs.HasKey("masterIsFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterIsFullscreen");

                if(localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }
            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");
                brightnessSlider.value = localBrightness;
                brightnessTextVal.text = localBrightness.ToString("0.0");
                //change brightness of actually game
            }
            if (PlayerPrefs.HasKey("masterMouseSensitivityX"))
            {
                float localXSens = PlayerPrefs.GetFloat("masterMouseSensitivityX");
                mouseXSensValue.text = localXSens.ToString("0.0");
                mouseXSenSlider.value = localXSens;
                menuController.mainMouseSensX = localXSens;
            }
            if (PlayerPrefs.HasKey("masterMouseSensitivityY"))
            {
                float localYSens = PlayerPrefs.GetFloat("masterMouseSensitivityY");
                mouseYSensValue.text = localYSens.ToString("0.0");
                mouseYSensSlider.value = localYSens;
                menuController.mainMouseSensY = localYSens;
            }
            if (PlayerPrefs.HasKey("masterInvertY"))
            {
                if (PlayerPrefs.GetInt("masterInvertY") == 1)
                {
                    invertYToggle.isOn = true;
                }
                else
                {
                    invertYToggle.isOn = false;
                }
            }

        }
    }
}
