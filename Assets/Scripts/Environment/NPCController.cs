using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] private Dialogue dialogue;
    private bool isDialogueActive = false;
    [SerializeField] private TMP_FontAsset font;


    public void Interact()
    {
        if (!isDialogueActive)
        {
            isDialogueActive = true;
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, font));
        }
    }

    private void OnDialogueEnd()
    {
        isDialogueActive = false;
    }

    // Subscribe to DialogueManager’s OnHideDialogue event in `Start` or `OnEnable`
    private void Start()
    {
        DialogueManager.Instance.OnHideDialogue += OnDialogueEnd;
    }
}
