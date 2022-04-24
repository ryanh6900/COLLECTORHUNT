using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public bool gameIsPaused;
    public GameObject inventoryUI;
    private Vector3 hidden = new Vector3(0, 0, 0);
    private Vector3 shown = new Vector3(1, 1, 1);

    private void Awake()
    {
        gameIsPaused = false;
        inventoryUI.transform.localScale = hidden;

    }
    void Update()
    {
        if (!gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                EnterInventory();
            }
            else
                ExitInventory();
        }
    }

    public void EnterInventory()
    {
        //gameIsPaused = true;
        inventoryUI.transform.localScale = shown;
        
    }

    public void ExitInventory()
    {

        //inventoryUI.SetActive(false);
        inventoryUI.transform.localScale = hidden;

    }
}
