using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeCanvas : MonoBehaviour
{
    private GameObject deathScreen;
    // Start is called before the first frame update
    void Start()
    {
        deathScreen = GameObject.FindWithTag("Deathscreen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Activates or deactivates the death screen
    public void ActivateDeath(bool yes) {
        if (yes) {
            deathScreen.SetActive(true);
        } else {
            deathScreen.SetActive(false);
        }
    }
}
