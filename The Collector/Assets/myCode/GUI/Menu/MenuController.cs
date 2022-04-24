using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuController : MonoBehaviour
{
    [Header("LoadingScenes")]
    //public string newGameLevel;
    public string exploreScene = "DeveloperScene";
    public string mainMenuScene = "MainMenuNew";
    //private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Audio Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private Slider mouseXSenSlider = null;
    [SerializeField] private TMP_Text mouseXSensValue = null;
    [SerializeField] private Slider mouseYSensSlider = null;
    [SerializeField] private TMP_Text mouseYSensValue = null;
    [SerializeField] private float defaultSensX = 4;
    [SerializeField] private float defaultSensY = 4;
    public float mainMouseSensX = 4.0f;
    public float mainMouseSensY = 4.0f;
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextVal = null;
    [SerializeField] private float defaultBrightness = 1.0f; //make brightness integer 0-100 then multiply by some fraction to change the engine brightness

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    [SerializeField] private Toggle fullScreenToggle = null;

    private int qualityLevel;
    private bool isFullScreen;
    private float brightnessLevel;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    int currentResolutionIndex;

    public bool gameIsPaused = false;

    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x "+ resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void TogglePause()
    {
        gameIsPaused = !gameIsPaused;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }
    public void AudioApply()
    {
        PlayerPrefs.SetFloat("masterVolume",AudioListener.volume);
        StartCoroutine(ConfimationBox());
        //Show Prompt when saved
    }
    public void SetXSens(float xSensitivity)
    {
        mainMouseSensX = xSensitivity;
        mouseXSensValue.text = xSensitivity.ToString("0.0");
    }
    public void SetYSens(float ySensitivity)
    {
        mainMouseSensY = ySensitivity;
        mouseYSensValue.text = ySensitivity.ToString("0.0");
    }
    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1); //inverts y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0); //does not invert y
        }
        PlayerPrefs.SetFloat("masterMouseSensitivityX",mainMouseSensX);
        PlayerPrefs.SetFloat("masterMouseSensitivityY",mainMouseSensY);
        StartCoroutine(ConfimationBox());
    }
    public void SetBrightness(float brightness)
    {
        brightnessLevel = brightness;
        brightnessTextVal.text = brightness.ToString("0.0");
    }

    public void SetFullscreen(bool _isFullscreen)
    {
        isFullScreen = _isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        qualityLevel = qualityIndex;
    }
    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", brightnessLevel);
        //change brightness with post processing

        PlayerPrefs.SetInt("masterOverallQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("masterIsFullscreen", isFullScreen ? 1 : 0);
        Screen.fullScreen = isFullScreen;

        StartCoroutine(ConfimationBox());
    }
    public void ResetBtn(string menuType)
    {
        if (menuType == "Graphics")
        {
            //Reset brightness value
            brightnessSlider.value = defaultBrightness;
            brightnessTextVal.text = defaultBrightness.ToString("0.0");
            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);
            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }
        if (menuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            AudioApply();
        }
        if(menuType == "Gameplay")
        {
            mouseXSensValue.text = defaultSensX.ToString("0.0");
            mouseXSenSlider.value = defaultSensX;
            mainMouseSensX = defaultSensX;
            mouseYSensValue.text = defaultSensY.ToString("0.0");
            mouseYSensSlider.value = defaultSensY;
            mainMouseSensY = defaultSensY;
            invertYToggle.isOn = false;
            GameplayApply();
        }
    }
    public IEnumerator ConfimationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
    public void ExploreModeDialogYes()
    {
        SceneManager.LoadScene(exploreScene);
    }

    //public void LoadGameDialogYes()
    //{
    //    if (PlayerPrefs.HasKey("SavedLevel"))
    //    {
    //        //levelToLoad = PlayerPrefs.GetString("SavedLevel");
    //        SceneManager.LoadScene(exploreScene);
    //    }
    //    else
    //    {
    //        noSavedGameDialog.SetActive(true);
    //    }
    //}
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}

