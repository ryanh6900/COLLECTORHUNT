using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSinglePlayerCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform humanSpawnPoint;
    public Transform witchSpawnPoint;
    Camera playerCam;
    void Start()
    {
        humanSpawnPoint = GameObject.Find("HumanSpawnPoint").GetComponent<Transform>();
        witchSpawnPoint = GameObject.Find("WitchSpawnPoint").GetComponent<Transform>();
        int selectedCharacter = PlayerPrefs.GetInt("selectedSinglePlayerCharacter");
        GameObject selectedCharacterPrefab = characterPrefabs[selectedCharacter];
        Transform playCharacterSpawnPoint = (selectedCharacterPrefab.CompareTag("Human")) ? humanSpawnPoint : witchSpawnPoint;
        Transform enemyPlayerSpawnPoint = (selectedCharacterPrefab.CompareTag("Human")) ? witchSpawnPoint : humanSpawnPoint;
        GameObject playCharacterSpawn = Instantiate(selectedCharacterPrefab,playCharacterSpawnPoint.position,Quaternion.identity);
        playerCam = playCharacterSpawn.GetComponentInChildren<Camera>();
        playerCam = Camera.main;
    }
 
}
