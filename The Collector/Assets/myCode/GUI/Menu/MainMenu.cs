using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu :MonoBehaviour //I really want to find a way to make this a static class...
{
    public InventoryObject playerInventory;
    public FirstPersonPlayer player; //I have to decide how I want to load player based on if the user starts a new game or loads from existing saved game.
    private void Start()
    {
        //Find a way to connect to Player before scene loads. //if I implement main menu as static this wont be necessary.
    }
    public void Play()
    {
        SceneManager.LoadScene("Nightmare");
    }

    public void Load()
    {
        //player.LoadPlayer();
        playerInventory.LoadInventory();
        SceneManager.LoadScene("DeveloperScene");
    }
    public void EnterDeveloperMode()
    {
        SceneManager.LoadScene("DeveloperScene");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Exit Btn Works");
    }
}
