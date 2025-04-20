using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CanvasScript : MonoBehaviour
{
    public GameObject MainScreenOverlay;
    public GameObject QPanel;
    public GameObject MainMenu;
    public GameObject PauseMenu;
    public GameObject BillBoardCloseup;

    private bool isPaused = false;
    public TMP_Text questionHistoryText;
    public QuestionGenerator questionGenerator;
    public Player player;
    // Start is called before the first frame update
   
    private void Start()
    {
        MainMenu.SetActive(true);
        QPanel.SetActive(false);
        player.DisableMovement();
        Time.timeScale = 0f;
        if (questionGenerator != null)
        {
            questionGenerator.UpdateMainScreenOverlay(); // Call the UpdateMainScreenOverlay method
        }

    }

    private void Update()
    {
        if (MainMenu.activeSelf) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
           
        }
    }


    //Main Menu
    public void StartGame()
    {
        MainMenu.SetActive(false);
        Time.timeScale = 1f;
        if (player != null)
        {
            player.EnableMovement();
        }
        MainScreenOverlay.SetActive(true);
        if (questionGenerator != null)
        {
            questionGenerator.UpdateMainScreenOverlay(); // Call the UpdateMainScreenOverlay method
        }
    }


    //Pause Menu
    public void OnPauseMenuOpen()
    {
        DisplayQuestionInfo();

    }
    public void DisplayQuestionInfo()
    {
        List<QuestionData> history = questionGenerator.GetQuestionHistoryValues();
        string historyText = "Question History:\n\n";

        foreach (var data in history)
        {
            historyText += $"Q: {data.question}\n";
            historyText += $"Your Answer: {data.playerAns}\n";
            historyText += $"Correct Answer: {data.correctAnswer}\n";
            historyText += $"Correct: {data.isCorrect}\n\n";
        }

        questionHistoryText.text = historyText;
    }

    public void OpenPauseMenu()
    {
        isPaused = true;
        OnPauseMenuOpen();
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        player.DisableMovement();
    }

    public void ClosePauseMenu()
    {
        isPaused = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;

        //this is to ensure players cant move if QPanel is open, as i was facing this issue before.
        if (!QPanel.activeSelf)
        {
            player.EnableMovement();

        }
        
    }

   

    //Question Panel
    public void QPanelClose()
    {
        if (QPanel != null)
        {
            QPanel.SetActive(false);
            Time.timeScale = 1f;
            player.EnableMovement();
        }

    }
    public void QPanelOpen()
    {
        QPanel.SetActive(true);
        player.DisableMovement();

        //// Set animator to Idle state
        player.animator.SetFloat("xVelocity", 0f);
        player.animator.SetFloat("yVelocity", 0f);


    }


    //billboard

    public void BillBoardClose()
    {
        if (BillBoardCloseup != null)
        {
            BillBoardCloseup.SetActive(false);
            Time.timeScale = 1f;
            player.EnableMovement();
        }

    }

}
