using System.Collections;
using UnityEngine;
using TMPro;

public class Narrator : MonoBehaviour
{
    public TMP_Text messageText;
    private Coroutine messageCoroutine;
    public AudioSource typeWriterSound;

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
    //uses coroutine and IEnumerator return type method for async operations
    private IEnumerator TypeWriterEffect(string msg)
    {
        messageText.text = "";
        if (typeWriterSound != null) {
            typeWriterSound.Play();
        }
        foreach (char c in msg)
        {
            messageText.text += c;
            yield return new WaitForSeconds(0.04f);
        }
        if (typeWriterSound != null)
        {
            typeWriterSound.Stop();
        }
        messageCoroutine = null;

    }

    public void HideMessage()
    {
        if (typeWriterSound != null && typeWriterSound.isPlaying)
        {
            typeWriterSound.Stop();
        }
        messageCoroutine = null;
    }
}
