using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : Singleton<CutsceneManager>
{
    [SerializeField] private PlayableDirector startingCutscene;

    [SerializeField] private Dialogue startingDialogue;
    private bool isDialogueActive = false;

    public enum CUTSCENE
    {
        NONE,
        STARTING,
        COUNT
    };

    public CUTSCENE currentCutscene;

    private void Start()
    {
        currentCutscene = CUTSCENE.STARTING;
        DialogueManager.Instance.OnHideDialogue += OnDialogueEnd;
    }

    public void HandleUpdate()
    {
        if (startingCutscene.state != PlayState.Playing)
        {
            GameController.Instance.state = GameState.Dialogue;
            if (!isDialogueActive)
            {
                isDialogueActive = true;
                StartCoroutine(DialogueManager.Instance.ShowDialogue(startingDialogue));
                GameController.Instance.ToggleConsole();
                currentCutscene = CUTSCENE.NONE;
            }

           
        }
    }

    private void OnDialogueEnd()
    {
        isDialogueActive = false;
        
    }

    
}
