//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Settings : MonoBehaviour
//{
//    public GameObject basicSettings; 
//    public GameObject basicButton;
//    //public GameObject MainMenuButtons;
//    public List<GameObject> MainMenuButtons;
//    public Slider volumeSlider;

//    public AudioSource backgroundMusicSource;


//    private bool settingsOpen = false;



//    private void Start()
//    {

//        // Hide settings panel at start
//        basicSettings.SetActive(false);

//        if (PlayerPrefs.HasKey("MasterVolume"))
//        {
//            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
//            volumeSlider.value = savedVolume;
//            backgroundMusicSource.volume = savedVolume;
//        }
//        else
//        {
//            volumeSlider.value = 1f;
//            backgroundMusicSource.volume = 1f;
//        }
//        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

//    }

//    public void ToggleSettings()
//    {
//        settingsOpen = !settingsOpen;

//        if (settingsOpen)
//        {

//            // After movement, show settings panel
//            basicSettings.SetActive(true);
//            //MainMenuButtons.SetActive(false);
//            foreach (GameObject button in MainMenuButtons)
//            {
//                button.SetActive(false);
//            }
//        }
//        else
//        {
//            // Hide settings panel first
//            basicSettings.SetActive(false);
//            //MainMenuButtons.SetActive(true);
//            foreach (GameObject button in MainMenuButtons)
//            {
//                button.SetActive(true);
//            }


//        }
//    }
//    private void OnVolumeChanged(float masterVolume)
//    {
//        backgroundMusicSource.volume = masterVolume; // Set global volume
//        PlayerPrefs.SetFloat("MasterVolume", masterVolume); // Save it
//        PlayerPrefs.Save();
//    }


//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtonController : MonoBehaviour
{
    public GameObject guidePanel;
    public GameObject settingsPanel;
    public List<GameObject> MainMenuButtons;
    public Narrator narrator;

    
    public Slider volumeSlider;
    public Slider soundEffectsSlider;
    public AudioSource backgroundMusicSource;
    public List<AudioSource> soundEffects;

    private enum MenuState { None, Guide, Settings }
    private MenuState currentState = MenuState.None;

    private void Start()
    {
        // Hide panels at the start
        guidePanel.SetActive(false);
        settingsPanel.SetActive(false);

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

        if (PlayerPrefs.HasKey("SoundEffects"))
        {
            float savedSoundEffectsVol = PlayerPrefs.GetFloat("SoundEffects");
            soundEffectsSlider.value = savedSoundEffectsVol;
            SetSoundEffectVolume(savedSoundEffectsVol);
        }
        else
        {
            soundEffectsSlider.value = 1f;
            SetSoundEffectVolume(1f);
        }


        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        soundEffectsSlider.onValueChanged.AddListener(OnSoundEffectChanged);

    }

    public void ToggleGuide()
    {
        if (currentState == MenuState.Guide)
        {
            // Close the guide panel
            guidePanel.SetActive(false);
            narrator.HideMessage();
            currentState = MenuState.None;
        }
        else
        {
            // Open the guide panel
            guidePanel.SetActive(true);
            narrator.ShowMessage("Welcome, Explorer!\r\n\r\nAnswer maths questions to earn points!\r\nRight answers give you points.\r\nWrong answers lose you health!\r\n\r\nReach Level 3 to find a hidden key in a secret cave.\r\nUse the key to unlock the Shop, where you can spend points on Health Bars or Fuel Bars.\r\n\r\nYou need 3 Fuel Bars to escape the planet.\r\nNo fuel means you’re stuck forever! No health means you perish...\r\n\r\nSolve questions, stay healthy, find the key, buy fuel - and escape!\r\n\r\nGood luck!");
            currentState = MenuState.Guide;
        }

        // Hide other panels and main menu buttons while Guide is open
        settingsPanel.SetActive(false);
        foreach (GameObject button in MainMenuButtons)
        {
            button.SetActive(currentState == MenuState.None);
        }
    }
    public void ToggleSettings()
    {
        // Toggle the Settings panel
        if (currentState == MenuState.Settings)
        {
            settingsPanel.SetActive(false);
            currentState = MenuState.None;
        }
        else
        {
            settingsPanel.SetActive(true);
            currentState = MenuState.Settings;
        }

        // Hide other panels and main menu buttons while Settings is open
        guidePanel.SetActive(false);
        narrator.HideMessage();
        foreach (GameObject button in MainMenuButtons)
        {
            button.SetActive(currentState == MenuState.None);
        }
    }

    public void BackButton()
    {
        // Handle the back button press logic
        if (currentState == MenuState.Guide || currentState == MenuState.Settings)
        {
            // Close the current panel (either Guide or Settings)
            if (currentState == MenuState.Guide)
            {
                guidePanel.SetActive(false);
            }
            else if (currentState == MenuState.Settings)
            {
                settingsPanel.SetActive(false);
            }

            // Show main menu buttons again
            foreach (GameObject button in MainMenuButtons)
            {
                button.SetActive(true);
            }

            // Reset the state to None
            currentState = MenuState.None;
        }
    }

    private void OnVolumeChanged(float masterVolume)
    {
        backgroundMusicSource.volume = masterVolume; // Set global volume
        PlayerPrefs.SetFloat("MasterVolume", masterVolume); // Save it
        PlayerPrefs.Save();
    }
    private void OnSoundEffectChanged(float volume)
    {
        SetSoundEffectVolume(volume);
        PlayerPrefs.SetFloat("SoundEffects", volume); // Save it
        PlayerPrefs.Save();
    }
    private void SetSoundEffectVolume(float volume)
    {
        foreach (AudioSource vol in soundEffects)
        {
            if (vol != null)
            {
                vol.volume = volume;
            }
        }
    }
}
