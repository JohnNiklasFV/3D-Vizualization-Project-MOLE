using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class ColorSelectionManager : MonoBehaviour
{
    public static ColorSelectionManager Instance;

    [Header("Color Selection Panel")]
    // Your coworker assigns this panel in the Inspector
    public GameObject colorSelectionPanel;

    [Header("Player Prompt Text")]
    // Text showing "Player 1 pick a color" etc.
    public TMP_Text playerPromptText;

    [Header("Color Buttons")]
    // Your coworker assigns these 4 buttons in the Inspector
    public Button redButton;
    public Button blueButton;
    public Button greenButton;
    public Button yellowButton;

    // Tracks which player is currently picking
    private int currentPickingPlayer = 1;
    private int totalPlayers = 2;

    // Stores the chosen colors in order
    // Index 0 = Player 1, Index 1 = Player 2 etc.
    private List<PlayerColor> chosenColors = new();

    // Tracks which colors are still available
    private List<PlayerColor> availableColors = new();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Hide panel by default — shown when player count is selected
        if (colorSelectionPanel != null)
            colorSelectionPanel.SetActive(false);

        // Hook up color buttons
        if (redButton != null)
            redButton.onClick.AddListener(() => OnColorSelected(PlayerColor.Red));
        if (blueButton != null)
            blueButton.onClick.AddListener(() => OnColorSelected(PlayerColor.Blue));
        if (greenButton != null)
            greenButton.onClick.AddListener(() => OnColorSelected(PlayerColor.Green));
        if (yellowButton != null)
            yellowButton.onClick.AddListener(() => OnColorSelected(PlayerColor.Yellow));
    }

    // =========================
    // START COLOR SELECTION
    // Called by MainMenu when player count is chosen
    // =========================
    public void StartColorSelection(int playerCount)
    {
        totalPlayers = playerCount;
        currentPickingPlayer = 1;
        chosenColors.Clear();

        // All colors available at start
        availableColors = new List<PlayerColor>
        {
            PlayerColor.Red,
            PlayerColor.Blue,
            PlayerColor.Green,
            PlayerColor.Yellow
        };

        // Show the color selection panel
        if (colorSelectionPanel != null)
            colorSelectionPanel.SetActive(true);

        UpdateUI();
    }

    // =========================
    // COLOR SELECTED
    // Called when a player clicks a color button
    // =========================
    private void OnColorSelected(PlayerColor color)
    {
        // Make sure the color is still available
        if (!availableColors.Contains(color)) return;

        // Save the choice
        chosenColors.Add(color);
        availableColors.Remove(color);

        Debug.Log($"Player {currentPickingPlayer} chose {color}");

        currentPickingPlayer++;

        // If all players have picked, start the game
        if (currentPickingPlayer > totalPlayers)
        {
            SaveAndLoadGame();
            return;
        }

        UpdateUI();
    }

    // =========================
    // UPDATE UI
    // Updates prompt text and disables unavailable color buttons
    // =========================
    private void UpdateUI()
    {
        // Update prompt text
        if (playerPromptText != null)
            playerPromptText.text = $"Player {currentPickingPlayer} — Pick a color";

        // Enable/disable buttons based on availability
        if (redButton != null)
            redButton.interactable = availableColors.Contains(PlayerColor.Red);
        if (blueButton != null)
            blueButton.interactable = availableColors.Contains(PlayerColor.Blue);
        if (greenButton != null)
            greenButton.interactable = availableColors.Contains(PlayerColor.Green);
        if (yellowButton != null)
            yellowButton.interactable = availableColors.Contains(PlayerColor.Yellow);
    }

    // =========================
    // SAVE AND LOAD
    // Saves all choices to PlayerPrefs then loads the game
    // =========================
    private void SaveAndLoadGame()
    {
        // Save player count
        PlayerPrefs.SetInt("PlayerCount", totalPlayers);

        // Save each player's color choice
        // Player 1 color = index 0, Player 2 = index 1 etc.
        for (int i = 0; i < chosenColors.Count; i++)
        {
            PlayerPrefs.SetInt($"Player{i + 1}Color", (int)chosenColors[i]);
            Debug.Log($"Saved Player {i + 1} color: {chosenColors[i]}");
        }

        PlayerPrefs.Save();

        // Load the game scene
        SceneManager.LoadSceneAsync(1);
    }
}