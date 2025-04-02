using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class GPTMathQuestionGenerator : MonoBehaviour
{
    [SerializeField] private string apiKey = "YOUR_API_KEY_HERE"; // Set this in the inspector or via GameManager
    [SerializeField] private string apiUrl = "https://api.openai.com/v1/chat/completions";
    [SerializeField] private string model = "gpt-3.5-turbo";

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    [Serializable]
    private class RequestData
    {
        public string model;
        public Message[] messages;
    }

    [Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    private class ResponseData
    {
        public Choice[] choices;
    }

    [Serializable]
    private class Choice
    {
        public Message message;
    }

    [Serializable]
    public class MathQuestion
    {
        public string question;
        public string answer;
        public string[] options;
    }

    public void GenerateMathQuestion(DifficultyLevel difficulty, Action<MathQuestion> callback)
    {
        StartCoroutine(GenerateMathQuestionCoroutine(difficulty, callback));
    }

    private IEnumerator GenerateMathQuestionCoroutine(DifficultyLevel difficulty, Action<MathQuestion> callback)
    {
        string prompt = GetPromptForDifficulty(difficulty);
        
        RequestData requestData = new RequestData
        {
            model = model,
            messages = new Message[]
            {
                new Message { role = "system", content = "You are a math teacher creating questions for a game. Respond ONLY with a JSON object containing 'question', 'answer' (the correct numerical answer), and 'options' (an array of 4 possible answers including the correct one)." },
                new Message { role = "user", content = prompt }
            }
        };

        string jsonData = JsonUtility.ToJson(requestData);
        
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || 
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                callback(GetFallbackQuestion(difficulty));
            }
            else
            {
                string response = request.downloadHandler.text;
                try
                {
                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(response);
                    string content = responseData.choices[0].message.content;
                    
                    // Extract JSON from the response
                    string jsonPattern = @"\{.*\}";
                    Match match = Regex.Match(content, jsonPattern, RegexOptions.Singleline);
                    
                    if (match.Success)
                    {
                        string jsonContent = match.Value;
                        MathQuestion question = JsonUtility.FromJson<MathQuestion>(jsonContent);
                        callback(question);
                    }
                    else
                    {
                        Debug.LogError("Failed to parse JSON from response: " + content);
                        callback(GetFallbackQuestion(difficulty));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error parsing response: " + e.Message);
                    callback(GetFallbackQuestion(difficulty));
                }
            }
        }
    }

    private string GetPromptForDifficulty(DifficultyLevel difficulty)
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                return "Generate an easy math question for elementary school students involving addition or subtraction with numbers between 1 and 20.";
            
            case DifficultyLevel.Medium:
                return "Generate a medium difficulty math question for middle school students involving multiplication or division with numbers between 1 and 100.";
            
            case DifficultyLevel.Hard:
                return "Generate a challenging math question for high school students involving exponents, brackets, or multi-step operations.";
            
            default:
                return "Generate a simple math question.";
        }
    }

    private MathQuestion GetFallbackQuestion(DifficultyLevel difficulty)
    {
        // Fallback questions in case API fails
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                return new MathQuestion
                {
                    question = "What is 7 + 8?",
                    answer = "15",
                    options = new string[] { "14", "15", "16", "17" }
                };
            
            case DifficultyLevel.Medium:
                return new MathQuestion
                {
                    question = "What is 8 × 7?",
                    answer = "56",
                    options = new string[] { "54", "56", "58", "63" }
                };
            
            case DifficultyLevel.Hard:
                return new MathQuestion
                {
                    question = "What is 2³ + 5 × 3?",
                    answer = "23",
                    options = new string[] { "21", "23", "25", "27" }
                };
            
            default:
                return new MathQuestion
                {
                    question = "What is 2 + 2?",
                    answer = "4",
                    options = new string[] { "3", "4", "5", "6" }
                };
        }
    }
}

