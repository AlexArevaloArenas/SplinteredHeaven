using System.Collections;
using UnityEngine;

public class Message : MonoBehaviour
{
    public TMPro.TextMeshProUGUI messageText; // Reference to the TextMeshProUGUI component for displaying messages
    public float messageSpeed = 0.05f; // Speed at which the message is revealed
    public float messageWaiting = 2f; // Speed at which the message is revealed
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        messageText.maxVisibleCharacters = 0; // Reset the maxVisibleCharacters to 0 to start revealing the message from the beginning
    }

    public void SetMessage(string message)
    {
        // Set the text of the messageText component to the provided message
        StartCoroutine(ShowMessage(message));
    }

    public IEnumerator ShowMessage(string message)
    {
        messageText.text = message; // Set the text of the messageText component to the provided message
        yield return new WaitForSeconds(messageWaiting); // Wait for a short duration before revealing the next character
        while (messageText.maxVisibleCharacters < message.Length)
        {
            // Incrementally reveal the message character by character
            messageText.maxVisibleCharacters++;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.messageSFX, Camera.main.transform.position); // Play the message sound effect
            yield return new WaitForSeconds(messageSpeed); // Wait for a short duration before revealing the next character
        }

    }
}
