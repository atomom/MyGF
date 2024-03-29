// Cape Guy, 2015. Use at your own risk.
using UnityEditor;
using UnityEngine;

/// <summary>
/// This script exits play mode whenever script compilation is detected during an editor update.
/// </summary>
[InitializeOnLoad] // Make static initialiser be called as soon as the scripts are initialised in the editor (rather than just in play mode).
public class CompileTool
{

    // Static initialiser called by Unity Editor whenever scripts are loaded (editor or play mode)
    static CompileTool()
    {
        Unused(_instance);
        _instance = new CompileTool();
    }

    private CompileTool()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    ~CompileTool()
    {
        EditorApplication.update -= OnEditorUpdate;
        // Silence the unused variable warning with an if.
        _instance = null;
    }

    // Called each time the editor updates.
    private static void OnEditorUpdate()
    {
        if (EditorApplication.isPlaying && EditorApplication.isCompiling)
        {
            Debug.Log("Exiting play mode due to script compilation.");
            EditorApplication.isPlaying = false;
        }
    }

    // Used to silence the 'is assigned by its value is never used' warning for _instance.
    private static void Unused<T>(T unusedVariable) { }

    private static CompileTool _instance = null;
}