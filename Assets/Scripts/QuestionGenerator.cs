using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

//this class holds question data for history tracking
public class QuestionData
{
    public string question;
    public string playerAns;
    public string correctAnswer;
    public bool isCorrect;

    //constructor method to set the values when storing a question attempt
    public QuestionData(string question, string playerAns, string correctAnswer, bool isCorrect)
    {
        this.question = question;
        this.playerAns = playerAns;
        this.correctAnswer = correctAnswer;
        this.isCorrect = isCorrect;
    }
}
public class QuestionGenerator : MonoBehaviour
{
    //TMP question and answer areas
    public TMP_Text questionText;
    public TMP_Text[] ansButtons;

    //timer and flags
    public TMP_Text timerText;
    private Coroutine timerCoroutine;
    private bool isAnswered = false;

    //MainScreenOverlay UI components
    public TMP_Text levelText;
    public TMP_Text pointsText;
    public TMP_Text healthBar;
    public Image healthBarFill;
    private float prevFill = -1f;
    public RawImage KeyImage;

    //fuel UI bar display
    public List<RawImage> fuelBars;

    //this makes (read-only) boolean HasKey public
    public bool HasKey => hasKey;

    //use List of QuestionData type, which contains question, playerAns, correctAns, and also bool isCorrect
    private List<QuestionData> questionHistory = new List<QuestionData>();

    //used to store determine repeated questions from maths box generator
    private HashSet<string> generatedQuestions = new HashSet<string>();




    //initialising some default values and flags
    private bool isGenerating = false;
    public int points = 0;
    public int level = 1;
    public int health = 10;
    public int maxHealth = 10;
    public bool hasKey = false;
    public bool correct = false;
    
    //references to UI panels and scripts
    public GameObject QPanel;
    public GameObject keyObject;
    public Player player;
    public Instructions billboard;
    public SpaceShip spaceShip;
    public CanvasScript canvasScript;
    public ShopController shopController;
    public CaveDoor caveDoor;
    public MathsBoxManager mathsBoxManager;


    //soundEffects
    public AudioSource correctSound;
    public AudioSource incorrectSound;
    public AudioSource timerTickSound;
    public AudioSource buzzerSound;

    //each maths box gameobject
    

    //adding api key from backend
    public string backendURL = "https://unitybackend.onrender.com";
    public string RetrievedApiKey { get; private set; }

    //private string apiKey = EnvLoaderAPIKey.GetEnv("API_KEY");
    private string correctAns = "";

    public void Start()
    {

        points = 0;
        level = 1;
        health = 10;
        maxHealth = 10;
        mathsBoxManager.RandomLocation();
        SetKeyFaded(!hasKey);
        UpdateFuelBars(0);
        UpdateHealthBarFill(health);
        UpdateMainScreenOverlay();

    }
    //starts process of generating question, called in other script
    public void GenerateQuestion()
    {
        if (!isGenerating)
        {
            StartCoroutine(CallGPTForQuestion());
        }
    }

    //calls backend server to receive new message based on level
    IEnumerator CallGPTForQuestion()
    {
        isGenerating = true;
        isAnswered = false;
        Coroutine loadingText = StartCoroutine(LoadingText());


        string prompt = "";
        //here prompts are set based on user level
        if (level == 1)
        {
            prompt = @"Generate a math question with random numbers using addition or subtraction only. Return in JSON format like:
                    { 
                      'question': 'What is 4 + 5?',
                      'correct': '9',
                      'wrong1': '7',
                      'wrong2': '12'
                    }";
        }
        else if (level == 2)
        {
            prompt = @"Generate a math question with random numbers using multiplication or division. Return JSON like:
                    { 
                      'question': 'What is 6 * 7?',
                      'correct': '42',
                      'wrong1': '36',
                      'wrong2': '48'
                    }";
        }
        else if (level == 3)
        {
            prompt = @"Generate a math question using brackets or indices with random numbers. Return JSON like:
                    { 
                      'question': 'What is (2 + 3)^2?',
                      'correct': '25',
                      'wrong1': '10',
                      'wrong2': '15'
                    }";
        }
        //retry in the case of receiving duplicate questions
        int retries = 5;
        JObject finalResponse = null;
        string finalQuestion = "";
        while (retries-- > 0)
        {
            //set the payload and convert it to string
            var payload = new JObject { ["prompt"] = prompt };
            var bodyRaw = System.Text.Encoding.UTF8.GetBytes(payload.ToString());

            //send POST request to backend server using message body contaning the specific prompt per request
            using (var request = new UnityWebRequest($"{backendURL}/ask", "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Backend error: {request.error}");
                    questionText.text = "Error generating question.";
                    isGenerating = false;
                    yield break;
                }

                JObject res = JObject.Parse(request.downloadHandler.text);
                string newQuestion = res["question"].ToString();
                
                //this is used to store repeated questions and see if they are being repeated, then retry
                if (!generatedQuestions.Contains(newQuestion))
                {
                    finalResponse = res;
                    finalQuestion = newQuestion;
                    generatedQuestions.Add(newQuestion);
                    break;
                }
                else
                {
                    Debug.Log("Duplicate question found, retrying...");
                    // Save in case all retries fail
                    finalResponse = res;
                    finalQuestion = newQuestion;
                }
            }
        }
        StopCoroutine(loadingText);
        // Use finalResponse, even if duplicate
        questionText.text = finalQuestion;
        correctAns = finalResponse["correct"].ToString();

        var answers = new List<string> {
        finalResponse["correct"].ToString(),
        finalResponse["wrong1"].ToString(),
        finalResponse["wrong2"].ToString()
    };

        for (int i = 0; i < answers.Count; i++)
        {
            int j = Random.Range(i, answers.Count);
            (answers[i], answers[j]) = (answers[j], answers[i]);
        }
        //assign answers onto UI buttons
        for (int k = 0; k < ansButtons.Length; k++)
            ansButtons[k].text = answers[k];

        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(StartTimer(10));

        isGenerating = false;
     
    }
    //shows animated loading text while waiting
    IEnumerator LoadingText()
    {
        while (true)
        {
            questionText.text = "Generating question.";
            yield return new WaitForSeconds(0.3f);
            questionText.text = "Generating question..";
            yield return new WaitForSeconds(0.3f);
            questionText.text = "Generating question...";
            yield return new WaitForSeconds(0.3f);
        }
    }
    //starts countdown timer for question
    IEnumerator StartTimer(int secs)
    {
        //check if qpanel is closed even after timer started

        int timeLeft = secs;
        while (timeLeft > 0)
        {
            if (!QPanel.activeSelf)
            //if panel was closed during timer, stop timer
            {
                Debug.Log("StartTimer aborted: QPanel was closed.");
                yield break;
            }
            timerText.text = $"Time: {timeLeft}";
            if (timerTickSound != null) timerTickSound.Play();
            yield return new WaitForSeconds(1f);
            timeLeft--;

            if (isAnswered)
            {

                yield break;

            }
        }
        //if timer runs out 
        timerText.text = "TIMES UP!";
        if (timerTickSound.isPlaying)
        {
            timerTickSound.Stop();
        }
        if (buzzerSound != null) buzzerSound.Play();
        yield return new WaitForSeconds(0.5f);
        health--;
        questionHistory.Add(new QuestionData(questionText.text, "None", correctAns, false));
        UpdateMainScreenOverlay();
        UpdateHealthBarFill(health);
        yield return new WaitForSeconds(1f);
        StartCoroutine(QPanelClose());



    }

    //called when answer is selected, it checks it here
    public void CheckAns(string chosenAns)
    {
        isAnswered = true;
        if (timerCoroutine != null && timerTickSound != null)
        {
            StopCoroutine(timerCoroutine);
            timerTickSound.Stop();

        }

        correct = false;
        if (questionText.text != "Generating question...")
        {
            if (chosenAns == correctAns)
            {
                correct = true;
                points++;
                if (correctSound != null) correctSound.Play();

            }
            else if (chosenAns != correctAns)
            {
                health--;
                UpdateHealthBarFill(health);
                if (incorrectSound != null) incorrectSound.Play();
            }
            //this now stores the relevant data of users' question and answer inside of a list
            questionHistory.Add(new QuestionData(questionText.text, chosenAns, correctAns, correct));
        }

        //update level based on points
        int previousLevel = level;
        if (points >= 0 && points <= 4)
        {
            level = 1;
        }
        else if (points >= 5 && points <= 9)
        {
            level = 2;
        }
        else if (points >= 10)
        {
            level = 3;
            GameObject.Find("CaveDoor").GetComponent<CaveDoor>().OpenCave();
        }
        //this ensures that even if you loose points, the levels dont change
        level = Mathf.Max(level, previousLevel);

        UpdateMainScreenOverlay();
        StartCoroutine(QPanelClose());

    }
    //called when any answer button is pressed
    public void OnButtonPressed(int i)
    {
        Debug.Log(ansButtons[i]);
        CheckAns(ansButtons[i].text);

    }

    //lets player manually close the question panel
    public void OnCloseButtonPressed()
    {
        //this ensures that even if you close the QPanel manually,
        //the timer is set to null so health doesnt decremenet even if you didnt answer
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;

        }
        if (timerTickSound.isPlaying)
        {
            timerTickSound.Stop();
        }
        isAnswered = true;
        //reset timer text
        timerText.text = "";
        StartCoroutine(QPanelClose());
    }

    //hides question panel and called to reset the UI after answer or timeout
    IEnumerator QPanelClose()
    {
        QPanel.SetActive(false);
        timerText.text = $"Time: ...";
        for (int k = 0; k < ansButtons.Length; k++)
            ansButtons[k].text = "...";
        spaceShip.canClick = true;
        billboard.canClick = true;
        Time.timeScale = 1f;
        yield return null;
        player.EnableMovement();
        mathsBoxManager.RandomLocation();
    }

    //updates main screen UI elements
    public void UpdateMainScreenOverlay()
    {
        levelText.text = $"Level: {level}";
        pointsText.text = $"Points: {points}";
        healthBar.text = $"{health}";
    }

    //returns list of type QuestionData with relevant q/a info
    public List<QuestionData> GetQuestionHistoryValues()
    {
        return questionHistory;
    }


    //this is for the key, which the info is passed onto FindKey script if CollectKey has been called
    //(meaning the key has been clicked, and collected, turning hasKey to true)
    public void SetKeyFaded(bool faded)
    {
        if (KeyImage != null)
        {
            Color iconColor = KeyImage.color;
            iconColor.a = faded ? 0.1f : 1f; // 0.5 = semi-transparent
            KeyImage.color = iconColor;
        }
    }

    //called to reflect key status on UI
    public void UpdateKeyImageColour()
    {
        SetKeyFaded(!hasKey);
    }

    //collecting key sets the flag to true
    public void CollectKey()
    {
        hasKey = true;
        UpdateKeyImageColour();
    }


    //fuel bar for main menu overlay
    public void UpdateFuelBars(int fuel)
    {
        foreach (var bar in fuelBars)
        {
            bar.enabled = false;
        }
        for (int i = 0; i < fuel; i++)
            if (i < fuelBars.Count)
            {
                fuelBars[i].enabled = true;
            }
    }

    //health bar fill
    public void UpdateHealthBarFill(int healthVal)
    {
        //if user dies, call the alternative ending panel
        if (healthVal == 0)
        {
            canvasScript.DiedEndMenu();
        }

        if (healthBarFill != null && maxHealth > 0)
        {
            float currentFill = (float)healthVal / maxHealth;

           
            healthBarFill.fillOrigin = (int)Image.OriginHorizontal.Left;
           

            healthBarFill.fillAmount = currentFill;
            prevFill = currentFill;
        }
    }

    //resets everything if the player restarts the game
    public void ResetPlayer()
    {
        health = maxHealth;
        points = 0;
        level = 1;
        questionHistory.Clear();
        hasKey = false;
        UpdateFuelBars(0);
        caveDoor.ResetCaveDoor();
        shopController.hasEnoughFuel = false;
        UpdateHealthBarFill(maxHealth);
        UpdateMainScreenOverlay();
        UpdateKeyImageColour();
        if (keyObject != null)
        {
            keyObject.SetActive(true);

            Renderer keyRenderer = keyObject.GetComponent<Renderer>();
            if (keyRenderer != null)
            {
                keyRenderer.enabled = true;
            }
        }

    }

}


