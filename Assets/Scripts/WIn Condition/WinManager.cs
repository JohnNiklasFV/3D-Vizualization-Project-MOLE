using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance;

    [Header("Win Screen UI")]
    // The panel that shows when someone wins — hidden by default
    public GameObject winPanel;

    // Text that displays which player won
    public TMP_Text winText;

    // Restart button that takes player back to main menu
    public Button restartButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Make sure win panel is hidden at game start
        if (winPanel != null)
            winPanel.SetActive(false);

        // Hook up restart button
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
    }
    
    void Update()
    {
        // Press W to test win screen — remove after testing
        if (Input.GetKeyDown(KeyCode.W))
        {
            TriggerWin(PlayerColor.Red);
        }
    }

    // =========================
    // TRIGGER WIN
    // Called by TurnManager when final burrow is filled
    // =========================
    public void TriggerWin(PlayerColor winner)
    {
        Debug.Log($"{winner} wins the game!");

        // Show win panel
        if (winPanel != null)
            winPanel.SetActive(true);

        // Set win text
        if (winText != null)
            winText.text = $"{winner} wins!";

        // Stop the game
        Time.timeScale = 0f;
        
        // Add to TriggerWin
        if (TurnIndicatorUI.Instance != null)
        TurnIndicatorUI.Instance.ClearText();
    }

    // =========================
    // RESTART
    // Takes player back to main menu
    // =========================
    private void OnRestartClicked()
    {
        // Resume time before loading new scene
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0); // Main Menu scene
    }
}