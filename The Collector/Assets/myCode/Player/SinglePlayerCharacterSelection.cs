using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SinglePlayerCharacterSelection : MonoBehaviour
{
    //public GameObject[] humanCharacters; //for future when I have more characters
    //public GameObject[] witchCharacters; //for future when I have more characters
    public GameObject warningDialog;
    public GameObject continueDialog;

    public GameObject humanDisplay;
    public GameObject witchDisplay;
    public GameObject[] characters;
    public int selectedPlayerCharacter;
    public int enemyAICharacter;
    public void Start()
    {
        selectedPlayerCharacter = -1;
        enemyAICharacter = -1;
        //humanDisplay = GameObject.Find("Human");
        //humanDisplay.SetActive(true);

        //witchDisplay = GameObject.Find("Witch");
        //witchDisplay.SetActive(true);
        //warningDialog = GameObject.Find("SelectionWarningDialog");
        //continueDialog.SetActive(false);
        //continueDialog = GameObject.Find("ExploreModeContinueDialog");
        //warningDialog.SetActive(false);
    }
    public void DisplayHuman()
    {
        humanDisplay.SetActive(true);
    }
    public void DisplayWitch()
    {
        witchDisplay.SetActive(true);
    }

    public void SelectHuman()
    {
        selectedPlayerCharacter = 0;
        enemyAICharacter = 1;
    }
    public void SelectWitch()
    {
        selectedPlayerCharacter = 1;
        enemyAICharacter = 0;
    }
    public void ResetCharacterSelection()
    {
        selectedPlayerCharacter = -1;
        enemyAICharacter = -1;
    }
    public void ConfirmCharacter()
    {
        if (selectedPlayerCharacter < 0) StartCoroutine(WarningDialog());
        else
        {
            PlayerPrefs.SetInt("selectedSinglePlayerCharacter", selectedPlayerCharacter);
            PlayerPrefs.SetInt("enemyAICharacter", enemyAICharacter);
            continueDialog.SetActive(true);
        }
    }

    public IEnumerator WarningDialog()
    {
        warningDialog.SetActive(true);
        yield return new WaitForSeconds(2);
        warningDialog.SetActive(false);
    }
}
