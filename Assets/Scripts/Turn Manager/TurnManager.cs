using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [Header("Player Setup")]
    public int playerCount = 2;

    // Hardcoded for now — player 1 = Red, player 2 = Blue
    private List<PlayerColor> playerOrder = new();
    private int currentPlayerIndex = 0;

    // Turn state
    private bool hasDrawnToken = false;
    private int currentSteps = -1;
    private bool hasMoved = false;

    public bool HasDrawnToken => hasDrawnToken;
    public int CurrentSteps => currentSteps;
    public PlayerColor CurrentPlayerColor => playerOrder[currentPlayerIndex];

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializePlayers();
    }

    private void InitializePlayers()
    {
        playerOrder.Clear();

        // Hardcoded 2 players for now
        // Expand this later for 3-4 players
        playerOrder.Add(PlayerColor.Red);
        playerOrder.Add(PlayerColor.Blue);

        // Initialize tokens for each player
        foreach (PlayerColor color in playerOrder)
        {
            TokenManager.Instance.InitializePlayer(color);
        }

        Debug.Log($"Game initialized with {playerOrder.Count} players");

        // Start first turn
        StartTurn();
    }

    private void StartTurn()
    {
        // Reset turn state
        hasDrawnToken = false;
        currentSteps = -1;
        hasMoved = false;

        // Reset token manager turn state
        TokenManager.Instance.ResetTurnState();

        Debug.Log($"--- {CurrentPlayerColor}'s turn ---");

        // Show tokens for current player
        TokenUIManager.Instance.ShowTokensForPlayer(CurrentPlayerColor);
    }

    // Called by TokenUIManager when a card is flipped
    public void OnTokenDrawn(int steps)
    {
        hasDrawnToken = true;
        currentSteps = steps;

        Debug.Log($"{CurrentPlayerColor} drew {steps} — select a piece to move");
    }

    // Called by PlayerMovement when a piece is selected
    // Returns true if the piece belongs to the current player
    public bool IsCurrentPlayer(PlayerColor color)
    {
        return color == CurrentPlayerColor;
    }

    // Called by TileSelector after a move is confirmed
    public void OnMoveMade(BoardField destination)
    {
        hasMoved = true;

        // Check if landed on a dot
        if (destination.fieldType == FieldType.Dot)
        {
            bool bonusGranted = TokenManager.Instance.TryUseDotBonus(CurrentPlayerColor);
            if (bonusGranted)
            {
                Debug.Log($"{CurrentPlayerColor} landed on a dot — draw another token!");
                hasDrawnToken = false;
                currentSteps = -1;
                TokenUIManager.Instance.ShowTokensForPlayer(CurrentPlayerColor);
                return;
            }
        }

        // Disable cards after move
        TokenUIManager.Instance.DisableAllCards();

        // Check win/layer transition conditions here later

        // End turn after a short delay
        Invoke(nameof(EndTurn), 1f);
    }

    private void EndTurn()
    {
        // Move to next player
        currentPlayerIndex = (currentPlayerIndex + 1) % playerOrder.Count;
        Debug.Log($"Turn ended — next up: {CurrentPlayerColor}");
        StartTurn();
    }

    // Called by PlayerMovement to check if moving is allowed
    public bool CanSelectPiece()
    {
        return hasDrawnToken && !hasMoved;
    }
}