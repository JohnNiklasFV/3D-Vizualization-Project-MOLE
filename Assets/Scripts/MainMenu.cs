using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayTwoPlayers()
    {
        if (ColorSelectionManager.Instance != null)
            ColorSelectionManager.Instance.StartColorSelection(2);
    }

    public void PlayThreePlayers()
    {
        if (ColorSelectionManager.Instance != null)
            ColorSelectionManager.Instance.StartColorSelection(3);
    }

    public void PlayFourPlayers()
    {
        if (ColorSelectionManager.Instance != null)
            ColorSelectionManager.Instance.StartColorSelection(4);
    }

    // =========================
    // ORIGINAL FUNCTIONS
    // =========================
    public void PlayGame()
    {
        // Default to 2 players if coming from original play button
        PlayerPrefs.SetInt("PlayerCount", 2);
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}