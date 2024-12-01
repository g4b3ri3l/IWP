using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            var assemblyName = new AssemblyName(args.Name);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName.Name}.dll");
            return File.Exists(path) ? Assembly.LoadFrom(path) : null;
        };

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

        state = GameState.Cutscene;

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
                playerController.animator.enabled = true;
                playerController.HandleUpdate();
                break;
            case GameState.Dialogue:
                playerController.animator.enabled = false;
                DialogueManager.Instance.HandleUpdate();
                break;
            case GameState.Console:
                playerController.animator.enabled = false;
                pythonConsole.inputField.ActivateInputField();
                EventSystem.current.SetSelectedGameObject(pythonConsole.inputField.gameObject);
                break;
            case GameState.Cutscene:
                CutsceneManager.Instance.HandleUpdate();
                break;
            default:
                break;
        }
    }

    public void ToggleConsole()
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


            EventSystem.current.SetSelectedGameObject(pythonConsole.inputField.gameObject);  

        }

        // Disable player movement while in the console
        playerController.AllowMovement = (state == GameState.FreeRoam);
    }
}
