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
    private Dictionary<PlayerColor, List<PlayerPiece>> playerPieces = new();
    private PlayerPiece dotBonusPiece = null;

    // Turn state
    private bool hasDrawnToken = false;
    private int currentSteps = -1;
    private bool hasMoved = false;
    


    public PlayerPiece DotBonusPiece => dotBonusPiece;
    public bool HasDrawnToken => hasDrawnToken;
    public int CurrentSteps => currentSteps;
    public PlayerColor CurrentPlayerColor => playerOrder[currentPlayerIndex];

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // TurnManager now waits for PlacementManager to call StartGame
        // before initializing players and turns
    }


        // Called by PlacementManager when all pieces are placed
    public void StartGame(Dictionary<PlayerColor, List<PlayerPiece>> pieces)
    {
        // Store pieces per player for forced-out rule later
        playerPieces = pieces;
        InitializePlayers();
    }
    private void InitializePlayers()
    {
        playerOrder.Clear();
        playerOrder.Add(PlayerColor.Red);
        playerOrder.Add(PlayerColor.Blue);

        foreach (PlayerColor color in playerOrder)
        {
            TokenManager.Instance.InitializePlayer(color);
        }

        Debug.Log($"Game started with {playerOrder.Count} players");
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
    public void OnMoveMade(BoardField destination, PlayerPiece piece)
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
                hasMoved = false;
                currentSteps = -1;
                dotBonusPiece = piece; // store the piece that landed on double dot
                TokenUIManager.Instance.ShowTokensForPlayer(CurrentPlayerColor);
                return;
            }
        }

        dotBonusPiece = null;

        // Disable cards after move
        TokenUIManager.Instance.DisableAllCards();

        // Check win/layer transition
        if (LayerTransitionManager.Instance != null)
        {
            if (LayerTransitionManager.Instance.AllBurrowsFilled())
            {
                if (LayerTransitionManager.Instance.CurrentLayer >= 4)
                {
                    Debug.Log("GAME OVER — final burrow filled!");
                    if (WinManager.Instance != null)
                        WinManager.Instance.TriggerWin(CurrentPlayerColor);
                    return;
                }
                else
                {
                    LayerTransitionManager.Instance.TransitionToNextLayer();
                    return;
                }
            }
        }

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

    public void OnLayerTransition(int newLayer)
    {
        Debug.Log($"Layer transition to {newLayer} — checking remaining players");

        // Check if any player has been completely eliminated
        foreach (var kvp in playerPieces)
        {
            bool hasAnyPiece = false;
            foreach (PlayerPiece piece in kvp.Value)
            {
                if (piece != null && piece.state != PieceState.Eliminated)
                {
                    hasAnyPiece = true;
                    break;
                }
            }

            if (!hasAnyPiece)
                Debug.Log($"{kvp.Key} has been eliminated from the game!");
        }

        // Continue with next turn
        currentPlayerIndex = 0;
        StartTurn();
    }
}