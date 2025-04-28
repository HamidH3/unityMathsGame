using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsPauseMenu : MonoBehaviour
{
    public GameObject controlPanel;
    public GameObject settingsPanel;
    public List<GameObject> questionHistoryObjects;
    public List<GameObject> controlInstructions;
    //public Narrator narrator;
    public Slider volumeSlider;
    public AudioSource backgroundMusicSource;


    private enum MenuState { None, Controls, Settings }
    private MenuState currentState = MenuState.None;

    private void Start()
    {
        // Hide panels at the start
        controlPanel.SetActive(false);
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

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void ToggleControls()
    {
        if (currentState == MenuState.Controls)
        {
            // Close the guide panel
            controlPanel.SetActive(false);
            currentState = MenuState.None;
        }
        else
        {
           
            controlPanel.SetActive(true);
            foreach (GameObject instructions in controlInstructions)
            {
                instructions.SetActive(true);
            }
            currentState = MenuState.Controls;
        }

        // Hide other panels and main menu buttons while Guide is open
        settingsPanel.SetActive(false);
        foreach (GameObject button in questionHistoryObjects)
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
        controlPanel.SetActive(false);
        foreach (GameObject button in questionHistoryObjects)
        {
            button.SetActive(currentState == MenuState.None);
        }
    }

    //public void BackButton()
    //{
    //    // Handle the back button press logic
    //    if (currentState == MenuState.Guide || currentState == MenuState.Settings)
    //    {
    //        // Close the current panel (either Guide or Settings)
    //        if (currentState == MenuState.Guide)
    //        {
    //            guidePanel.SetActive(false);
    //        }
    //        else if (currentState == MenuState.Settings)
    //        {
    //            settingsPanel.SetActive(false);
    //        }

    //        // Show main menu buttons again
    //        foreach (GameObject button in MainMenuButtons)
    //        {
    //            button.SetActive(true);
    //        }

    //        // Reset the state to None
    //        currentState = MenuState.None;
    //    }
    //}

    public void ResetPanelOnClose()
    {
        controlPanel.SetActive(false);
        settingsPanel.SetActive(false);
        foreach (GameObject button in questionHistoryObjects)
        {
            button.SetActive(true);
        }
        //currentState = MenuState.None;
    }

    private void OnVolumeChanged(float masterVolume)
    {
        backgroundMusicSource.volume = masterVolume; // Set global volume
        PlayerPrefs.SetFloat("MasterVolume", masterVolume); // Save it
        PlayerPrefs.Save();
    }
}
