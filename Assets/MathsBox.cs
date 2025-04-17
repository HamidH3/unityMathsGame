
using UnityEngine;

public class MathChest : MonoBehaviour
{
    public GameObject QuestionGenerator;  // Reference to the GameObject containing QuestionGenerator

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered chest. Generating question...");

            // Get the QuestionGenerator from the specified GameObject
            QuestionGenerator questionGenerator = QuestionGenerator.GetComponent<QuestionGenerator>();
            questionGenerator?.GenerateQuestion();
        }
    }
}
