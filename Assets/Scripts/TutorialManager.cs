using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int currentStage = 0;

    
    

    private void Update()
    {
        if (GameController.Instance.state == GameState.Console)
        {

            switch (currentStage)
            {
                case 0:
                    if (PythonConsole.Instance.outputText.text == "20\r\n")
                    {
                        PythonConsole.Instance.inputField.text = "";
                        currentStage++;
                    }
                    break;

                case 1:
                    PythonConsole.Instance.SetText("#Great Work! Let’s up the difficulty a little bit! In programming, there are things called “Variables”.\n " +
                                                   "#These are basically how you can store data and assign values to these titles!\n" +
                                                   "#For example: x = 5, y = -2!\n" +
                                                   "# YOUR GOAL IS TO OUTPUT NUMBER -52\n" +
                                                   "x = 50\n" +
                                                   "y = -62\n" +
                                                   "print(x + y)");

                    break;
            }
        }

        

    }
}
