using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartupDisplay : MonoBehaviour
{
    [SerializeField]
    private string[] messageArray;

    [SerializeField]
    private Sound messageSound;

    [SerializeField]
    private TextMeshProUGUI messageText;

    private int _messageIndex;

    public void NewMessage()
    {
        GameManager.audioManager.PlaySound(messageSound);
        StartCoroutine(TypeSentence());
    }

    private IEnumerator TypeSentence()
    {
        string sentence = messageArray[_messageIndex];

        messageText.text = "";

        foreach (char character in sentence.ToCharArray())
        {
            messageText.text += character;
            yield return new WaitForSeconds(0.06f);
        }

        _messageIndex++;
    }
}
