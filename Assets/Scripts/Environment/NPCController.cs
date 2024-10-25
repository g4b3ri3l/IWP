using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] private Dialogue dialogue;
    private bool isDialogueActive = false;


    public void Interact()
    {
        if (!isDialogueActive)
        {
            isDialogueActive = true;
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
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
