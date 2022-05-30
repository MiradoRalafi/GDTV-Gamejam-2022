using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    #region PARAMETERS

    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    private string[] dialogueLines;
    [SerializeField]
    private List<int> hideIndexes;
    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    private OnDialogueEnd onDialogueEnded;

    #endregion

    #region CACHES



    #endregion

    #region STATES

    private int currentDialogueLineIndex = 0;

    #endregion

    private void Start()
    {
        dialogueText.text = dialogueLines[currentDialogueLineIndex];
    }

    public void ContinueDialogue()
    {
        if (hideIndexes.Contains(currentDialogueLineIndex))
        {
            dialoguePanel.SetActive(false);
        }
        currentDialogueLineIndex++;
        if (currentDialogueLineIndex == dialogueLines.Length)
        {
            onDialogueEnded.Invoke();
            dialoguePanel.SetActive(false);
            return;
        }
        dialogueText.text = dialogueLines[currentDialogueLineIndex];
    }

    public void ResumeDialogue()
    {
        dialoguePanel.SetActive(true);
        currentDialogueLineIndex++;
        dialogueText.text = dialogueLines[currentDialogueLineIndex];
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && dialoguePanel.activeSelf)
        {
            ContinueDialogue();
        }
    }
}

[System.Serializable]
public class OnDialogueEnd : UnityEvent { }