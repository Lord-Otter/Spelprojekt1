using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textBox;
    [SerializeField] private Button nextButton;
    [SerializeField] List<string> dialogue;

    [SerializeField] private UnityEvent onStartDialogue;
    [SerializeField] private UnityEvent onEndDialogue;

    private int currentIndex = -1;


    public void NextDialogue()
    {
        // If dialogue wasn't started: run the onStartDialogue-event, and add this dialogue to the Next-Button
        if (currentIndex < 0)
        {
            onStartDialogue?.Invoke();
        //    nextButton?.onClick.AddListener(NextDialogue);
        }
        
        // Move to the next item in the list of dialogue-lines
        currentIndex++;
        // If we're not at the end of the list yet: update text in text-box
        if (currentIndex < dialogue.Count)
        {
            textBox.text = dialogue[currentIndex];
        }
        // If we are at the end of the list: run the onEndDialogue-event, reset the index (so that the dialogue can be restarted) and remove this dialog from the next-button
        else
        {
            textBox.text = "";
            onEndDialogue?.Invoke();
            currentIndex = -1;
            nextButton?.onClick.RemoveListener(NextDialogue);
        }
    }
}