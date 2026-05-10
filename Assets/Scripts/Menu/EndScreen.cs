using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Loads up the main menu scene, 0 is the main menu build index
public class EndScreen : MonoBehaviour
{
    public void BackMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

     //public void Replay()
    //{
        //
    //}
    
      public void QuitGame()
    {
        Application.Quit();
    }
}

