using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        string savePath = Application.persistentDataPath + "/PlayerData.json";

        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
        }

        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
