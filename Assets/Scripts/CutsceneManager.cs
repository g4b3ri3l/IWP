using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : Singleton<CutsceneManager>
{
    [SerializeField] private PlayableDirector startingCutscene;

    [SerializeField] private Dialogue startingDialogue;
    [SerializeField] private TMP_FontAsset startingFont;

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
                StartCoroutine(DialogueManager.Instance.ShowDialogue(startingDialogue, startingFont));
               // GameController.Instance.ToggleConsole();
               currentCutscene = CUTSCENE.NONE;
               PlayerUIManager.Instance.currentText.text = PlayerUIManager.Instance.interactText;

                PlayerUIManager.Instance.ShowText();
            }


        }
    }

    private void OnDialogueEnd()
    {
        isDialogueActive = false;
        PlayerUIManager.Instance.currentText.text = PlayerUIManager.Instance.moveText;
        PlayerUIManager.Instance.ShowText();

    }


}
