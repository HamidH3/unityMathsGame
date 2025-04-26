using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject basicSettings; 
    public GameObject basicButton;   
    public GameObject MainMenuButtons;
    public Slider volumeSlider;

    public AudioSource backgroundMusicSource;


    private bool settingsOpen = false;



    private void Start()
    {

        // Hide settings panel at start
        basicSettings.SetActive(false);

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            volumeSlider.value = savedVolume;
            backgroundMusicSource.volume = savedVolume;
        }
        else
        {
            volumeSlider.value = 1f;
            backgroundMusicSource.volume = 1f;
        }
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

    }

    public void ToggleSettings()
    {
        settingsOpen = !settingsOpen;

        if (settingsOpen)
        {

            // After movement, show settings panel
            basicSettings.SetActive(true);
            MainMenuButtons.SetActive(false);
        }
        else
        {
            // Hide settings panel first
            basicSettings.SetActive(false);
            MainMenuButtons.SetActive(true);

            
        }
    }
    private void OnVolumeChanged(float masterVolume)
    {
        backgroundMusicSource.volume = masterVolume; // Set global volume
        PlayerPrefs.SetFloat("MasterVolume", masterVolume); // Save it
        PlayerPrefs.Save();
    }


}
