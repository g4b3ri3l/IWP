using UnityEngine;
using UnityEngine.UI;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using TMPro;
using System;
using System.IO;
using IronPython.Modules;

public class PythonConsole : Singleton<PythonConsole>
{
    public TMP_InputField inputField;  // Multi-line input for Python code
    public TMP_Text outputText;        // Text to display output or errors
    private ScriptEngine pythonEngine;
    private ScriptScope pythonScope;

    [SerializeField] private GameObject playerObject;
    public Button runButton;           // Button to run the script
    private MemoryStream outputStream;
    private StreamWriter streamWriter;



    void Start()
    {
        // Initialize the Python engine and scope
        pythonEngine = Python.CreateEngine();
        pythonScope = pythonEngine.CreateScope();

        // Preload game objects or functions into Python scope
        pythonScope.SetVariable("player", playerObject);
        pythonScope.SetVariable("move_player", (Action<Vector3>)MovePlayer);

        // Setup memory stream for capturing print() output
        outputStream = new MemoryStream();
        streamWriter = new StreamWriter(outputStream);
        streamWriter.AutoFlush = true;
        pythonEngine.Runtime.IO.SetOutput(outputStream, streamWriter);

        // Ensure inputField can take multi-line input
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                ExecutePythonCode();
            }
        }
       
    }

    public void SetText(string text)
    {
        inputField.text = text;
    }

    // Method to move player to a new position using Python
    public void MovePlayer(Vector3 newPosition)
    {
        playerObject.transform.position = newPosition;
    }

    // Function to execute Python code entered in the InputField
    public void ExecutePythonCode()
    {
        string pythonCode = inputField.text;

        // Sanitize input by removing problematic characters like vertical tabs (\v) and other control characters
        pythonCode = pythonCode.Replace("\v", ""); // Remove vertical tabs
        pythonCode = pythonCode.Replace("\r\n", "\n"); // Normalize line endings (Windows style to Unix style)
        pythonCode = pythonCode.Replace("\r", "\n"); // Normalize other line endings

        try
        {
            // Clear previous output
            outputText.text = "";

            // Setup a new memory stream for capturing print() output each time the script is run
            using (var outputStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(outputStream))
            {
                streamWriter.AutoFlush = true;
                pythonEngine.Runtime.IO.SetOutput(outputStream, streamWriter);

                // Execute the code
                pythonEngine.Execute(pythonCode, pythonScope);

                // Read output from the memory stream
                outputStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(outputStream, System.Text.Encoding.UTF8))
                {
                    string printedOutput = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(printedOutput))
                    {
                        outputText.text = printedOutput;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            // Catch and display any errors from Python execution
            outputText.text = "Error: " + ex.Message;
        }
    
    }   

    private void OnDestroy()
    {
        // Properly clean up the streams when the object is destroyed
        streamWriter?.Dispose();
        outputStream?.Dispose();
    }
}
