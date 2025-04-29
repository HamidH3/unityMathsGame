using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


public class QuestionData
{
    public string question;
    public string playerAns;
    public string correctAnswer;
    public bool isCorrect;

    //constructor method to initalise the data values
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

    //timer
    public TMP_Text timerText;
    private Coroutine timerCoroutine;
    private bool isAnswered = false;
    
    //MainScreenOverlay
    public TMP_Text levelText;
    public TMP_Text pointsText;
    public TMP_Text healthBar;
    public Image healthBarFill;
    private float prevFill = -1f;
    public RawImage KeyImage;

    public List<RawImage> fuelBars;

    public bool HasKey => hasKey;//this makes (read-only) boolean HasKey public

    //use List of QuestionData type, which contains question, playerAns, correctAns, and also bool isCorrect
    private List<QuestionData> questionHistory = new List<QuestionData>();
    public bool correct = false;

    //initialising values
    private bool isGenerating = false;
    public int points = 0;
    public int level = 1;
    public int health = 25;
    public int maxHealth = 25;
    private bool hasKey = false;

    public GameObject QPanel;
    public Player player;
    public Instructions billboard;
    public SpaceShip spaceShip;


    //soundEffects
    public AudioSource correctSound;
    public AudioSource incorrectSound;
    public AudioSource timerTickSound;
    public AudioSource buzzerSound;




    private string apiKey = EnvLoaderAPIKey.GetEnv("API_KEY");
    private string correctAns = "";
    //private string apiKey = ""; // <-- replace this with your OpenAI key

    public void Start()
    {
        //mathsBoxManager.ActivateRandomBox();
        

        Debug.Log("Loaded API Key: " + apiKey);
        points = 0;
        level = 1;
        health = 25;
        maxHealth = 25;
        SetKeyFaded(!hasKey);
        UpdateFuelBars(0);
        UpdateHealthBarFill(health);
        UpdateMainScreenOverlay();

       
    }
    public void GenerateQuestion()
    {
        //Debug.Log("Generating question...");
        if (!isGenerating)
        {
            //spawns.PauseSpawning();
            StartCoroutine(CallGPTForQuestion());

        }
    }

    IEnumerator CallGPTForQuestion()
    {
        isGenerating = true;
        questionText.text = "Generating question...";
        isAnswered = false;


        string prompt = "";

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
        var requestJson = new JObject
        {
            ["model"] = "gpt-3.5-turbo",
            ["messages"] = new JArray
    {
        new JObject
        {
            ["role"] = "user",
            ["content"] = prompt
        }
    }
        };

        string requestBody = requestJson.ToString();

        using (UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("GPT API error: " + request.error);
                questionText.text = "Error generating question.";
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                JObject result = JObject.Parse(jsonResponse);
                string content = result["choices"][0]["message"]["content"].ToString();

                //res holds the full result in json format of gpt generated answer
                JObject res = JObject.Parse (content);

                questionText.text = res["question"].ToString();
                correctAns = res["correct"].ToString();

                string[] answers = new string[]
                {
                    res["correct"].ToString(),
                    res["wrong1"].ToString(),
                    res["wrong2"].ToString()
                };
                //switch answer positions randomly here:
                for (int i = answers.Length - 1; i > 0; i--)
                {
                    int randomNum = Random.Range(0, i + 1);
                    (answers[i], answers[randomNum]) = (answers[randomNum], answers[i]);
                }

                for (int j = 0; j < ansButtons.Length; j++)
                {
                    ansButtons[j].text = answers[j];
                }
                //check if you closed the qpanel before timer started
                if (!QPanel.activeSelf)
                {
                    Debug.Log("QPanel was closed before timer start. Timer will not run.");
                    isGenerating = false;
                    yield break;
                }

                //here start the timer when the question and answers are generated (when this method
                //is called in GenerateQuestion, which is directly called in MathsBox.cs)
                if (timerCoroutine != null)
                {

                    StopCoroutine(timerCoroutine);
                }
                if (!QPanel.activeSelf)
                {
                    isGenerating = false;
                    yield break; // don't run timer if question panel is closed
                }
                timerCoroutine = StartCoroutine(StartTimer(10));
            }
            isGenerating = false;
        }
    }

    IEnumerator StartTimer(int secs)
    {
        //check if qpanel is closed even after timer started
       


        int timeLeft = secs;
        while ( timeLeft> 0) 
        {
            if (!QPanel.activeSelf)
                //if panel was closed during timer, stop timer
            {
                Debug.Log("StartTimer aborted: QPanel was closed.");
                //timerTickSound.Stop();
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
        //mathsBoxManager.SwapToNewBox();
        //spawns.RespawnChest();


    }

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
            //this now stores the relevant data of our questions inside of a list
            questionHistory.Add(new QuestionData(questionText.text, chosenAns, correctAns, correct));
        }

        int previousLevel = level;
        if (points >= 0 && points <=4 )
        {
            level = 1;
        }
        else if (points >= 5 && points <=9)
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

    public void OnButtonPressed(int i)
    {
        Debug.Log(ansButtons[i]);
        CheckAns(ansButtons[i].text);
        //spawns.EndQAndRespawn();
        //spawns.ResumeSpawning();
    }
    public void OnCloseButtonPressed()
    {
        //this ensures that even if you close the QPanel manually,
        //the timer is set to null so a health doesnt decremenet even if you didnt answer
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

    IEnumerator QPanelClose()
    {
        QPanel.SetActive(false);
        spaceShip.canClick = true;
        billboard.canClick = true;
        Time.timeScale = 1f;
        yield return null;
        player.EnableMovement();
    }

    public void UpdateMainScreenOverlay()
    {
        levelText.text = $"Level: {level}";
        pointsText.text = $"Points: {points}";
        healthBar.text = $"{health}";
    }

    public List<QuestionData> GetQuestionHistoryValues()
    {
        //returns list of type QuestionData with relevant q/a info
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
    public void UpdateKeyImageColour()
    {
        SetKeyFaded(!hasKey);
    }

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
        if (healthBarFill != null && maxHealth > 0)
        {
            float currentFill = (float)healthVal / maxHealth; 

            if (prevFill >= 0f)
            {
                if (currentFill > prevFill)
                {
                    // Health increased: fill from left
                    healthBarFill.fillOrigin = (int)Image.OriginHorizontal.Right;
                }
                else if (currentFill < prevFill)
                {
                    // Health decreased: fill from right
                    healthBarFill.fillOrigin = (int)Image.OriginHorizontal.Left;
                }
            }

            healthBarFill.fillAmount = currentFill;
            prevFill = currentFill;
        }
    }




}


