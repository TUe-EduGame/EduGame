using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using UnityEngine.TextCore.Text;
using System.Threading;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;

public class CodeCheck : MonoBehaviour
{
    Thread checkThread;

    [SerializeField]
    Button selectFileBtn;

    bool finished = true;

    public void Update()
    {
        // Busy-waiting  :(
        if (finished && !selectFileBtn.enabled)
        {
            checkThread.Join();
            checkThread = null;
            selectFileBtn.enabled = true;
        }
    }

    public void SelectFile()
    {
        finished = false;

        selectFileBtn.enabled = false;
        Assert.AreEqual(null, checkThread);

        GetFile();
    }

    void GetFile()
    {
        string filePath = EditorUtility.OpenFilePanel("Select the file you want to check", "", "c,cpp,py");

        UnityEngine.Debug.Log(filePath);

        if (filePath != null && filePath != "" && checkThread == null)
        {
            checkThread = new Thread(() => RunFile(filePath));
            checkThread.Start();
        }
    }

    void RunFile(string filePath)
    {
        if (Path.GetExtension(filePath) != ".py")
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
        var testProcess = Process.Start(testProcessInfo);

        testProcess.WaitForExit();
        testProcess.Close();

        int testTime = System.Environment.TickCount - startTime;

        UnityEngine.Debug.Log($"Time taken to run a test script: {testTime}ms");

        //start a process to run the file
        var processInfo = new ProcessStartInfo("python.exe", filePath)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true
        };

        startTime = System.Environment.TickCount;

        var process = Process.Start(processInfo);

        string processOutput = process.StandardOutput.ReadToEnd();
        string processError = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            UnityEngine.Debug.Log("Script executed successfully:\n" + processOutput);
        }
        else
        {
            UnityEngine.Debug.LogError($"Error while executing script:\n{processError}");
        }

        process.WaitForExit();
        process.Close();

        int processTime = System.Environment.TickCount - startTime;

        UnityEngine.Debug.Log($"Time taken: {processTime - testTime}ms");

        string folderPath = Path.GetDirectoryName(filePath);
        string outputPath = Path.Combine(folderPath, "out.txt");

        if (File.Exists(outputPath))
        {
            SHA512Managed sha512 = new SHA512Managed();
            FileStream stream = File.OpenRead(outputPath);
            byte[] hash = sha512.ComputeHash(stream);
            stream.Close();

            UnityEngine.Debug.Log(BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant());
        }
        else
        {
            UnityEngine.Debug.Log("The python script did not create an output file.");
        }

        finished = true;
    }
}
