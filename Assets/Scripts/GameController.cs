using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public enum GameState { FreeRoam, Dialogue, Console, Cutscene }
public class GameController : Singleton<GameController>
{
    [SerializeField] PlayerController playerController;
    [SerializeField] private GameObject consoleUI;
    [SerializeField] private PythonConsole pythonConsole;

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

        consoleUI.SetActive(false);

    }

    private IEnumerator ReturnToFreeRoamAfterDelay()
    {
        yield return new WaitForSeconds(playerController.interactionCooldown); // Match this to `interactionCooldown` in `PlayerController`
        if (state == GameState.Dialogue)
            state = GameState.FreeRoam;
    }

    private void Update()
    {
        // Toggle console UI with the backtick key
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleConsole();
        }

        switch (state)
        {
            case GameState.FreeRoam:
                playerController.HandleUpdate();
                break;
            case GameState.Dialogue:
                DialogueManager.Instance.HandleUpdate();
                break;
            case GameState.Console:
                break;
            default:
                break;
        }
    }

    private void ToggleConsole()
    {
        if (state == GameState.Console)
        {
            // Close the console
            state = GameState.FreeRoam;
            consoleUI.SetActive(false);
        }
        else
        {
            // Open the console
            state = GameState.Console;
            consoleUI.SetActive(true);
            pythonConsole.inputField.ActivateInputField();
            EventSystem.current.SetSelectedGameObject(pythonConsole.inputField.gameObject);  // Only set this when console is activated

        }

        // Disable player movement while in the console
        playerController.AllowMovement = (state == GameState.FreeRoam);
    }
}
