using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseGameMenu : MonoBehaviour
{
    public bool gameIsPaused;
    public Canvas blurBackground;
    public GameObject pauseMenuUI;
    public GameObject player;
    public GameObject flashlight;
    public InventoryObject playerInventory;
    void Awake()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        player.GetComponent<FirstPersonPlayer>().enabled = true;
    }
    void Update()
    {
        blurBackground.enabled = gameIsPaused;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void SaveGame()
    {
        playerInventory.SaveInventory();
        player.GetComponent<FirstPersonPlayer>().SavePlayer();
    }
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0f;
        player.GetComponent<FirstPersonPlayer>().enabled = false;
        //flashlight.GetComponent<Light>().enabled = false;
        //Cursor.visible = true;
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        player.GetComponent<FirstPersonPlayer>().enabled = true;
        player.GetComponent<FirstPersonPlayer>().GetComponent<AudioSource>().mute = false;
        //flashlight.GetComponent<Light>().enabled = true;
    }
    public void quitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
