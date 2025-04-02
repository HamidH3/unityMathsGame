using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Settings")]
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private int bulletsPerCorrectAnswer = 5;
    
    [Header("Question Settings")]
    [SerializeField] private float questionTimeLimit = 10f;
    [SerializeField] private int easyQuestionsThreshold = 3;
    [SerializeField] private int mediumQuestionsThreshold = 7;
    
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GPTMathQuestionGenerator questionGenerator;
    [SerializeField] private EnemySpawner enemySpawner;

    // Game state
    private int currentHealth;
    private int correctAnswers = 0;
    private int playerLevel = 1;
    private GPTMathQuestionGenerator.MathQuestion currentQuestion;
    private bool isGameOver = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Find references if not set
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
        
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        
        if (questionGenerator == null)
        {
            questionGenerator = FindObjectOfType<GPTMathQuestionGenerator>();
        }
        
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        currentHealth = startingHealth;
        correctAnswers = 0;
        playerLevel = 1;
        isGameOver = false;
        
        // Update UI
        uiManager?.UpdateHealth(currentHealth);
        uiManager?.UpdateBulletCount(0);
        uiManager?.UpdateLevelDisplay(playerLevel);
        
        // Set time scale to normal
        Time.timeScale = 1;
    }

    public void ShowMathQuestion()
    {
        GPTMathQuestionGenerator.DifficultyLevel difficulty = GetCurrentDifficulty();
        
        questionGenerator.GenerateMathQuestion(difficulty, question => {
            currentQuestion = question;
            uiManager.ShowMathQuestion(question, questionTimeLimit);
        });
    }

    private GPTMathQuestionGenerator.DifficultyLevel GetCurrentDifficulty()
    {
        if (correctAnswers >= mediumQuestionsThreshold)
        {
            return GPTMathQuestionGenerator.DifficultyLevel.Hard;
        }
        else if (correctAnswers >= easyQuestionsThreshold)
        {
            return GPTMathQuestionGenerator.DifficultyLevel.Medium;
        }
        else
        {
            return GPTMathQuestionGenerator.DifficultyLevel.Easy;
        }
    }

    public void CheckAnswer(string selectedAnswer)
    {
        bool isCorrect = selectedAnswer == currentQuestion.answer;
        
        if (isCorrect)
        {
            // Reward player
            correctAnswers++;
            playerController.AddBullets(bulletsPerCorrectAnswer);
            
            // Update level if needed
            UpdatePlayerLevel();
        }
        
        // Hide question panel
        uiManager.HideMathQuestion();
    }

    public void OnQuestionTimeUp()
    {
        // Hide question panel without giving reward
        uiManager.HideMathQuestion();
    }

    private void UpdatePlayerLevel()
    {
        int newLevel = 1 + (correctAnswers / 3); // Level up every 3 correct answers
        
        if (newLevel > playerLevel)
        {
            playerLevel = newLevel;
            uiManager.UpdateLevelDisplay(playerLevel);
            
            // Increase game difficulty
            float difficultyMultiplier = 1 + (playerLevel * 0.2f); // 20% harder per level
            enemySpawner.AdjustSpawnRate(difficultyMultiplier);
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        if (isGameOver) return;
        
        currentHealth -= damage;
        uiManager.UpdateHealth(currentHealth);
        
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        
        // Stop enemy spawning
        enemySpawner.SetSpawningActive(false);
        
        // Show game over UI
        uiManager.ShowGameOver(correctAnswers);
        
        // Pause game
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateBulletCount(int count)
    {
        uiManager.UpdateBulletCount(count);
    }
}

