using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json.Linq;

public class QuestionGenerator : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text[] ansButtons;

    public TMP_Text levelText;
    public TMP_Text pointsText;

    private bool isGenerating = false;
    private int points = 0;
    private int level = 1;

    private string correctAns = "";
    private string apiKey = ""; // <-- replace this with your OpenAI key

    public void GenerateQuestion()
    {
        Debug.Log("Generating question...");
        if (!isGenerating)
        {
            StartCoroutine(CallGPTForQuestion());

        }
    }

    IEnumerator CallGPTForQuestion()
    {
        isGenerating = true;
        questionText.text = "Generating question...";

        //string prompt = @"Generate a math question with difficulty:
        //- Easy: addition/subtraction with random numbers,
        //- Medium: multiplication/division with random numbers,
        //- Hard: with brackets and indices.
        //Return JSON like:
        //{ 
        //  'question': 'What is 4 + 5?',
        //  'correct': '9',
        //  'wrong1': '7',
        //  'wrong2': '12'
        //}";

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
            }
            isGenerating = false;
        }
    }

    public void CheckAns(string chosenAns)
    {
        if (chosenAns == correctAns)
        {
            points++;
        }
        else
        {
            return;
        }
        while (points > 0 && level < 4)
        {
            if (points >= 5)
            {
                level = 1;
            }
            else if (points >= 10)
            {
                level = 2;
            }
            else if (points >= 15)
            {
                level = 3;
            }

        }
    }

    public void OnButtonPressed(int i)
    {
        CheckAns(ansButtons[i].text);
    }

    public void UpdateMainScreenOverlay()
    {
        levelText.text = $"Level: {level}";
        pointsText.text = $"Points: {points}";
    }

}


