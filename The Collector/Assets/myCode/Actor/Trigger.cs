using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class Trigger : MonoBehaviour
{
    public GameObject Scare;

    public bool played;
    public AudioClip scareSound;
    public AudioSource scareSource;
    void Start()
    {
        played = false;
        scareSource = GetComponent<AudioSource>();
        Scare.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
            Scare.SetActive(true);
            PlaySound(); 
    }

    private void OnTriggerExit(Collider collider)
    {
        DestroyObject();
    }
    async void DestroyObject()
    {
        await Task.Delay(3000);
        Scare.SetActive(false);
        Debug.Log("Destroy Scare Called");
    }
    void PlaySound()
    {
        
        if (!played)
        {
            scareSource.PlayOneShot(scareSound);
            played = true;
        }
    }
}
