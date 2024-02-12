using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    AudioClip buttonClick;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void NewGame()
    {
        string savePath = Application.persistentDataPath + "/PlayerData.json";

        audioSource.PlayOneShot(buttonClick, 1);

        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
        }



        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        audioSource.PlayOneShot(buttonClick, 1);
        Application.Quit();
    }
}
