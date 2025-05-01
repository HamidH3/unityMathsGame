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
    public GameObject ShopPanel;
    public GameObject EndMenu;
    public GameObject diedEndMenu;



    private bool isPaused = false;
    public TMP_Text questionHistoryText;
    public QuestionGenerator questionGenerator;
    public Player player;
    public SpaceShip spaceShip;
    public ControlsPauseMenu controlsPauseMenu;
    public Instructions billboard;
    public Narrator narrator;
    public Animator animator;
    public EndMenu endMenu;

    private void Start()
    {
        MainMenu.SetActive(true);
        animator = GetComponent<Animator>();
        animator.Play("mainmenubckground");
        if (MainMenu != null)
            MainMenu.SetActive(true);

        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator != null)
        {
            // Check if animator has "mainmenubckground" state
            animator.Play("mainmenubckground");
        }
        else
        {
            Debug.LogWarning("Animator component missing from Canvas!");
        }
        EndMenu.SetActive(false);
        QPanel.SetActive(false);
        PauseMenu.SetActive(false);
        player.DisableMovement();
        Time.timeScale = 0f;
        if (questionGenerator != null)
        {
            questionGenerator.UpdateMainScreenOverlay(); // Call the UpdateMainScreenOverlay method
        }

    }

    private void Update()
    {
        if (MainMenu.activeSelf || EndMenu.activeSelf || diedEndMenu.activeSelf)
        {
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
        //here reset the game, so player location, and narrator speech
        MainMenu.SetActive(false);
        Time.timeScale = 1f;
        if (player != null)
        {
            player.EnableMovement();
        }
        MainScreenOverlay.SetActive(true);
        if (questionGenerator != null)
        {
            questionGenerator.UpdateMainScreenOverlay();
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
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
            if (data.isCorrect)
            {
                historyText += $"Correct\n\n";
            }
            else
            {
                historyText += $"Incorrect\n\n";
            }
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
        spaceShip.canClick = false;
        billboard.canClick = false;

    }

    public void ClosePauseMenu()
    {
        isPaused = false;
        if (controlsPauseMenu != null)
        {
            controlsPauseMenu.ResetPanelOnClose();
        }
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;

        spaceShip.canClick = true;
        billboard.canClick = true;


        //this is to ensure players cant move if QPanel is open, as i was facing this issue before.
        if (!QPanel.activeSelf)
        {
            player.EnableMovement();

        }

    }



    //Question Panel
    public void QPanelClose()
    {
        questionGenerator.OnCloseButtonPressed();

    }
    public void QPanelOpen()
    {
        QPanel.SetActive(true);
        player.DisableMovement();
        spaceShip.canClick = false;
        billboard.canClick = false;



        //// Set animator to Idle state
        player.animator.SetFloat("xVelocity", 0f);
        player.animator.SetFloat("yVelocity", 0f);


    }
    //End Menu
    public void EndMenuOpen()
    {
        EndMenu.SetActive(true);
        endMenu.InitializeEndMenu();
        player.DisableMovement();
        spaceShip.canClick = false;
        billboard.canClick = false;
    }
    public void PlayAgainButton()
    {
        EndMenu.SetActive(false);
        QPanel.SetActive(false);
        diedEndMenu.SetActive(false);
        MainMenu.SetActive(true);
        narrator.HideMessage();

        //reset game state

        if (questionGenerator != null)
        {
            questionGenerator.ResetPlayer();

        }
        //questionGenerator.hasKey = false;
        spaceShip.canClick = true;
        billboard.canClick = true;
    }

    //alternative end menu called when users health runs out
    public void DiedEndMenu()
    {
        diedEndMenu.SetActive(true);
        narrator.ShowMessage("Unfortunately your health was too low... Better luck next time!");
        player.DisableMovement();
        spaceShip.canClick = false;
        billboard.canClick = false;
    }


    //billboard

    public void BillBoardClose()
    {
        if (BillBoardCloseup != null)
        {
            BillBoardCloseup.SetActive(false);
            spaceShip.canClick = true;
            billboard.canClick = true;


            Time.timeScale = 1f;
            player.EnableMovement();
        }

    }


    //shop
    public void ExitShop()
    {

        if (ShopPanel != null)
        {
            ShopPanel.SetActive(false);
            spaceShip.canClick = true;
            narrator.HideMessage();

            Time.timeScale = 1f;
            player.EnableMovement();
        }

    }

}
