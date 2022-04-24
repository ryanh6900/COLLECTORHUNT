using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Threading.Tasks;
public class Flashlight : MonoBehaviour
{
    public bool lightOn;
    public float lightDrain;
    public float batteryLife;
    private int maxBatteryLife = 100;
    public Light flashLightSource;
    public AudioClip soundTurnOn;
    public AudioClip soundTurnOff;
    public AudioClip replaceBatterySound;
    public AudioSource soundSource;
    public PauseGameMenu pause;
    void Start()
    {
        batteryLife = maxBatteryLife;
        flashLightSource = GetComponent<Light>();
        soundSource = GetComponent<AudioSource>();
        pause = GameObject.FindGameObjectWithTag("InGameMenu").GetComponent<PauseGameMenu>();
    }
    void Update()
    {
       if (!pause.gameIsPaused){ 
        if (lightOn && batteryLife > 0)
        {
            batteryLife -= Time.deltaTime * lightDrain;
            flashLightSource.intensity = batteryLife * 0.50f;
        }

        if (batteryLife <= 0)
        {
            batteryLife = 0;
            lightOn = false;
        }
        if (batteryLife > maxBatteryLife)
                batteryLife = 100;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (lightOn)
                lightOn = false;
            else if (!lightOn && batteryLife > 0)
                lightOn = true;
            toggleFlashLight();
            toggleFlashLightSFX();
        }
     }
    }
  
    public void ChangeBattery(float amount)
    {
        //await Task.Delay(1000);
        AlterEnergy(amount);
        soundSource.PlayOneShot(replaceBatterySound);
    }
    public void toggleFlashLight()
    {
        flashLightSource.enabled = lightOn;
    }

    public void toggleFlashLightSFX()
    {
        if (flashLightSource.enabled)
            soundSource.PlayOneShot(soundTurnOn);
        else
           soundSource.PlayOneShot(soundTurnOff);
    }
    public void AlterEnergy(float amount)
    {
        batteryLife = Mathf.Clamp(batteryLife + amount, 0, 100);
    }

}
