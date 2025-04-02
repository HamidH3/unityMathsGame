using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI bulletCountText;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Header("Math Question Panel")]
    [SerializeField] private GameObject mathQuestionPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TextMeshProUGUI[] answerTexts;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerFill;
    
    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button restartButton;

    private Coroutine timerCoroutine;

    private void Start()
    {
        // Initialize UI
        mathQuestionPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        
        // Set up button listeners
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
        }
    }

    public void UpdateLevelDisplay(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"Level: {level}";
        }
    }

    public void UpdateBulletCount(int count)
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = $"Bullets: {count}";
        }
    }

    public void UpdateHealth(int health)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {health}";
        }
    }

    public void ShowMathQuestion(GPTMathQuestionGenerator.MathQuestion question, float timeLimit)
    {
        // Set question text
        questionText.text = question.question;
        
        // Shuffle and set answer options
        string[] shuffledOptions = ShuffleOptions(question.options, question.answer);
        for (int i = 0; i < answerTexts.Length; i++)
        {
            if (i < shuffledOptions.Length)
            {
                answerTexts[i].text = shuffledOptions[i];
                answerButtons[i].gameObject.SetActive(true);
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
        
        // Show panel
        mathQuestionPanel.SetActive(true);
        
        // Start timer
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(RunTimer(timeLimit));
        
        // Pause game
        Time.timeScale = 0;
    }

    private string[] ShuffleOptions(string[] options, string correctAnswer)
    {
        // Make sure the correct answer is included
        bool hasCorrectAnswer = false;
        foreach (string option in options)
        {
            if (option == correctAnswer)
            {
                hasCorrectAnswer = true;
                break;
            }
        }
        
        if (!hasCorrectAnswer && options.Length > 0)
        {
            options[0] = correctAnswer;
        }
        
        // Shuffle options
        for (int i = 0; i < options.Length; i++)
        {
            string temp = options[i];
            int randomIndex = Random.Range(i, options.Length);
            options[i] = options[randomIndex];
            options[randomIndex] = temp;
        }
        
        return options;
    }

    private IEnumerator RunTimer(float timeLimit)
    {
        float timeRemaining = timeLimit;
        
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(timeRemaining / timeLimit);
            
            // Update timer display
            timerText.text = $"{Mathf.Ceil(timeRemaining)}s";
            timerFill.fillAmount = normalizedTime;
            
            yield return null;
        }
        
        // Time's up
        GameManager.Instance.OnQuestionTimeUp();
    }

    private void OnAnswerSelected(int index)
    {
        if (index < answerTexts.Length)
        {
            string selectedAnswer = answerTexts[index].text;
            GameManager.Instance.CheckAnswer(selectedAnswer);
        }
    }

    public void HideMathQuestion()
    {
        mathQuestionPanel.SetActive(false);
        
        // Resume game
        Time.timeScale = 1;
        
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    public void ShowGameOver(int finalScore)
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Final Score: {finalScore}";
    }
}

