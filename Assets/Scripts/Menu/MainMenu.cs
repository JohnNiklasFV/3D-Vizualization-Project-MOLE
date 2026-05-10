using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Loads up the main scene, 1 is the MainScene build index
public class MainMenu : MonoBehaviour
{
    public void Playgame()
    {
        SceneManager.LoadSceneAsync(1);
    }

      public void QuitGame()
    {
        Application.Quit();
    }
}

