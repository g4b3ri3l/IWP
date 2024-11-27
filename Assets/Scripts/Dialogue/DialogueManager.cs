using System.Collections;
using System.Collections.Generic;
using IKVM.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private int lettersPerSecond;

    public event System.Action OnShowDialogue;
    public event System.Action OnHideDialogue;

    private Dialogue dialogue;
    private int currentLine = 0;
    private bool isTyping;
    

    public IEnumerator ShowDialogue(Dialogue dialogue, TMP_FontAsset font)
    {
        dialogueText.font = font;

        yield return new WaitForEndOfFrame();

        OnShowDialogue?.Invoke();

        this.dialogue = dialogue;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialogue.Lines.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            }
            else
            {
                dialogueBox.SetActive(false);
                currentLine = 0;
                OnHideDialogue?.Invoke();
            }
        }
    }

    public IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        isTyping = false;
    }
}
