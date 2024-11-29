using System;
using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField] private List<String> text = new List<string>();
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private TMP_Text currentText;

    private int currentTextIndex = 0;


    private void Start()
    {
        popUpPanel.SetActive(false);

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (GameController.Instance.state == GameState.FreeRoam && Input.GetKeyDown(KeyCode.Z))
        {
          
            ShowNextText();
            
            
        }
       
    }

    private void Update()
    {
        if (popUpPanel.activeSelf && Input.GetKeyDown(KeyCode.Z))
        {
            ShowNextText();
        }
    }

    private void ShowNextText()
    {
        popUpPanel.SetActive(true);

        if (currentTextIndex < text.Count)
        {
            currentText.text = text[currentTextIndex];
            currentTextIndex++;
        }
        else
        {
            // Reset or close popup when all texts are shown
            popUpPanel.SetActive(false);
            currentTextIndex = 0;
            GameController.Instance.ToggleConsole();
            PythonConsole.Instance.SetText("# YOUR GOAL IS TO OUTPUT THE NUMBER 20\n#CTRL+R TO RUN THE SCRIPT\nprint(15 + 3)");
        }
    }
}
