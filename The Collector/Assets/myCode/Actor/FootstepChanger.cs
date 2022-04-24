using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepChanger : MonoBehaviour
{
    [SerializeField] public TerrainChecker checker;
    public FirstPersonPlayer player;
    private string currentSurfaceTag;
    private string currentTerrainLayer;
    public MovementSoundContainer[] terrainSoundContainers;

    void Awake()
    {
       checker = new TerrainChecker();
       player = GetComponent<FirstPersonPlayer>();
    }
    public void CheckSurfaceTag()
    {
        if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 3))
        {
            //Only continues if player is touching a tagged surface
            if (hit.collider.tag == "Untagged") return;

            currentSurfaceTag = hit.collider.tag;

            foreach (MovementSoundContainer container in terrainSoundContainers)
            {
                if (currentSurfaceTag == container.name)
                {
                    player.ChangeFootsteps(container);
                    //Debug.Log("Added " + container.name);
                }
            }
        }
    }
    public void CheckTerrainLayer()
    {
        if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 3))
        {
            //only continues if player is touching terrain
            if (hit.collider.GetComponent<Terrain>() == null) return;

            Terrain t = hit.transform.GetComponent<Terrain>();
            currentTerrainLayer = checker.GetLayerName(transform.position, t);

            foreach (MovementSoundContainer container in terrainSoundContainers)
             {
                if (currentTerrainLayer == container.name)
                {
                    player.ChangeFootsteps(container);
                    //Debug.Log("Added " + container.name);
                }
             }      
        }
    }
}
