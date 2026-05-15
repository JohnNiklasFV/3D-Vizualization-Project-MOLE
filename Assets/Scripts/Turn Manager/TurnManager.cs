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
    public void StartGame(Dictionary<PlayerColor, List<PlayerPiece>> pieces, int count)
    {
        // Store pieces per player for forced-out rule later
        playerPieces = pieces;
        playerCount = count;
        InitializePlayers();
    }
    private void InitializePlayers()
    {
        playerOrder.Clear();
        playerOrder.Add(PlayerColor.Red);
        if (playerCount >= 2) playerOrder.Add(PlayerColor.Blue);
        if (playerCount >= 3) playerOrder.Add(PlayerColor.Green);
        if (playerCount >= 4) playerOrder.Add(PlayerColor.Yellow);

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

        // Add to end of StartTurn
        if (TurnIndicatorUI.Instance != null)
        TurnIndicatorUI.Instance.ShowTurnText(CurrentPlayerColor);
    }

    // Called by TokenUIManager when a card is flipped
    public void OnTokenDrawn(int steps)
    {
        hasDrawnToken = true;
        currentSteps = steps;

        Debug.Log($"{CurrentPlayerColor} drew {steps} — select a piece to move");

        // If dot bonus is active, check if the dot piece can actually make the move
        // If not, skip the bonus turn automatically
        if (dotBonusPiece != null)
        {
            List<BoardField> validMoves = TileSelector.Instance.GetValidDestinations(dotBonusPiece, steps);
            if (validMoves.Count == 0)
            {
                Debug.Log($"Dot bonus piece cannot make the move — skipping bonus turn");
                dotBonusPiece = null;
                TokenUIManager.Instance.DisableAllCards();
                Invoke(nameof(EndTurn), 1f);
            }
        }
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

            // Skip players with no remaining pieces
        int safetyCounter = 0;
        while (!HasAnyPieces(CurrentPlayerColor))
        {
            Debug.Log($"{CurrentPlayerColor} has no pieces — skipping turn");
            currentPlayerIndex = (currentPlayerIndex + 1) % playerOrder.Count;
            safetyCounter++;

            // If we've gone through all players, someone wins
            if (safetyCounter >= playerOrder.Count)
            {
                // Find last remaining player
                foreach (PlayerColor color in playerOrder)
                {
                    if (HasAnyPieces(color))
                    {
                        Debug.Log($"{color} is the last player standing — they win!");
                        if (WinManager.Instance != null)
                            WinManager.Instance.TriggerWin(color);
                        return;
                    }
                }
                return;
            }
        }

        Debug.Log($"Turn ended — next up: {CurrentPlayerColor}");
        StartTurn();
    }

    private bool HasAnyPieces(PlayerColor color)
    {
        if (!playerPieces.ContainsKey(color)) return false;

        foreach (PlayerPiece piece in playerPieces[color])
        {
            if (piece != null && piece.state != PieceState.Eliminated)
                return true;
        }
        return false;
    }

    // Called by PlayerMovement to check if moving is allowed
    public bool CanSelectPiece()
    {
        return hasDrawnToken && !hasMoved;
    }

    public void OnLayerTransition(int newLayer)
    {
        Debug.Log($"Layer transition to {newLayer} — checking remaining players");

        PlayerColor? lastPlayer = null;
        int activePlayers = 0;

        foreach (var kvp in playerPieces)
        {
            bool hasAnyPiece = HasAnyPieces(kvp.Key);
            if (hasAnyPiece)
            {
                activePlayers++;
                lastPlayer = kvp.Key;
            }
            else
            {
                Debug.Log($"{kvp.Key} has been eliminated from the game!");
            }
        }

        // If only one player remains they win immediately
        if (activePlayers == 1 && lastPlayer.HasValue)
        {
            Debug.Log($"{lastPlayer.Value} is the last player standing — they win!");
            if (WinManager.Instance != null)
                WinManager.Instance.TriggerWin(lastPlayer.Value);
            return;
        }

        // Continue with next turn
        currentPlayerIndex = 0;
        StartTurn();
    }
}