using System.Collections;
using UnityEngine;
using TMPro;

public class Narrator : MonoBehaviour
{
    public TMP_Text messageText;
    private Coroutine messageCoroutine;

    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }
            messageCoroutine = StartCoroutine(TypeWriterEffect(message));
        }
    }

    private IEnumerator TypeWriterEffect(string msg)
    {
        messageText.text = "";
        foreach (char c in msg)
        {
            messageText.text += c;
            yield return new WaitForSeconds(0.04f);
        }
        messageCoroutine = null;
        //yield return new WaitForSeconds(3f);


        //messageText.text = "";
    }
}
