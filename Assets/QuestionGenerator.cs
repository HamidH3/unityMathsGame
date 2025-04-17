using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json.Linq;

public class QuestionGenerator : MonoBehaviour
{
    public TMP_Text questionText;

    private string apiKey=""; // <-- replace this with your OpenAI key

    public void GenerateQuestion()
    {
        StartCoroutine(CallGPTForQuestion());
    }

    IEnumerator CallGPTForQuestion()
    {
        string prompt = "Generate a single school-friendly math question in one sentence. Choose randomly from Easy (addition/subtraction), Medium (multiplication/division), or Hard (with brackets). Only return the question sentence. Do not include answers.";

        string requestBody = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}]}";

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
                string content = result["choices"][0]["message"]["content"].ToString().Trim();

                questionText.text = content;
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            FindObjectOfType<QuestionGenerator>().GenerateQuestion();
        }
    }
}
