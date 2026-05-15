using UnityEngine;
using TMPro;

public class TurnIndicatorUI : MonoBehaviour
{
    public static TurnIndicatorUI Instance;

    [Header("UI Reference")]
    public TMP_Text turnText;

    void Awake()
    {
        Instance = this;
    }

    // =========================
    // PLACEMENT PHASE
    // Shows whose turn it is to place and how many pieces remaining
    // =========================
    public void ShowPlacementText(PlayerColor color, int piecesRemaining)
    {
        if (turnText != null)
            turnText.text = $"{color} — Place a piece ({piecesRemaining} remaining)";
    }

    // =========================
    // GAME PHASE
    // Shows whose turn it is to play
    // =========================
    public void ShowTurnText(PlayerColor color)
    {
        if (turnText != null)
            turnText.text = $"{color}'s Turn";
    }

    // =========================
    // CLEAR
    // Called when game ends
    // =========================
    public void ClearText()
    {
        if (turnText != null)
            turnText.text = "";
    }
}