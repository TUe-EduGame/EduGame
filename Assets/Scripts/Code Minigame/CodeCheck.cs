using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using UnityEngine.TextCore.Text;

public class CodeCheck : MonoBehaviour
{
    string file;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectFile()
    {
        file = EditorUtility.OpenFilePanel("Select the file you want to check", "", "c,cpp,py");

        UnityEngine.Debug.Log(file);

        if(file != null && file != "")
            RunFile();
    }

    public void RunFile()
    {
        if(Path.GetExtension(file) != ".py")
        {
            UnityEngine.Debug.LogError("File type not implemented yet");
            return;
        }

        //check how much does it take to run a test python script'
        var testProcessInfo = new ProcessStartInfo("python.exe", "--version")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
        };

        int startTime = System.Environment.TickCount;
        var testProcess =  Process.Start(testProcessInfo);

        testProcess.WaitForExit();
        testProcess.Close();

        int testTime = System.Environment.TickCount - startTime;

        UnityEngine.Debug.Log($"Time taken to run a test script: {testTime}ms");

        //start a process to run the file
        var processInfo = new ProcessStartInfo("python.exe", file)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true
        };

        startTime = System.Environment.TickCount;

        var process = Process.Start(processInfo);

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            UnityEngine.Debug.Log("Script executed successfully:\n" + output);
        }
        else
        {
            UnityEngine.Debug.LogError($"Error while executing script:\n{error}");
        }

        process.WaitForExit();
        process.Close();

        int processTime = System.Environment.TickCount - startTime;

        UnityEngine.Debug.Log($"Time taken: {processTime - testTime}ms");
    }
}
