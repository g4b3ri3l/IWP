using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum GameState { FreeRoam, Dialogue, Console, Cutscene }
public class GameController : Singleton<GameController>
{
    [SerializeField] PlayerController playerController;

    public GameState state;

    private void Start()
    {
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.Dialogue;
        };

        DialogueManager.Instance.OnHideDialogue += () =>
        {
            // Add a slight delay before setting the state back to FreeRoam
            StartCoroutine(ReturnToFreeRoamAfterDelay());
        };
    }

    private IEnumerator ReturnToFreeRoamAfterDelay()
    {
        yield return new WaitForSeconds(playerController.interactionCooldown); // Match this to `interactionCooldown` in `PlayerController`
        if (state == GameState.Dialogue)
            state = GameState.FreeRoam;
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.FreeRoam:
                playerController.HandleUpdate();
                break;
            case GameState.Dialogue:
                DialogueManager.Instance.HandleUpdate();
                break;
            default:
                break;
        }
    }
}
