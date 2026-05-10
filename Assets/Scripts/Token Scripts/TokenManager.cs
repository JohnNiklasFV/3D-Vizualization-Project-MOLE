using UnityEngine;
using System.Collections.Generic;

public class TokenManager : MonoBehaviour
{
    public static TokenManager Instance;

    // The base set of tokens every player gets
    private readonly int[] baseTokens = new int[] { 1, 2, 2, 3, 3, 4 };

    // Tracks remaining tokens per player
    private Dictionary<PlayerColor, List<int>> remainingTokens = new();

    // The token drawn this turn
    private int currentDrawnToken = -1;
    private bool hasDrawnThisTurn = false;

    // Dot bonus tracking
    private bool hasUsedDotThisTurn = false;

    public int CurrentDrawnToken => currentDrawnToken;
    public bool HasDrawnThisTurn => hasDrawnThisTurn;
    public bool HasUsedDotThisTurn => hasUsedDotThisTurn;

    void Awake()
    {
        Instance = this;
    }

    // Call this when setting up each player at game start
    public void InitializePlayer(PlayerColor color)
    {
        remainingTokens[color] = new List<int>(baseTokens);
        Debug.Log($"Tokens initialized for {color}");
    }

    // Returns the list of remaining token values for a player
    // UI uses this to know how many face-down cards to show
    public List<int> GetRemainingTokens(PlayerColor color)
    {
        if (!remainingTokens.ContainsKey(color))
            return new List<int>();

        return remainingTokens[color];
    }

    // Called when a player clicks a face-down card
    // index = which card slot they clicked (0-5)
    public int DrawToken(PlayerColor color, int index)
    {
        if (hasDrawnThisTurn && !IsDrawingBonusToken())
        {
            Debug.Log("Already drawn a token this turn!");
            return -1;
        }

        if (!remainingTokens.ContainsKey(color))
        {
            Debug.Log($"No tokens found for {color}");
            return -1;
        }

        List<int> tokens = remainingTokens[color];

        if (index < 0 || index >= tokens.Count)
        {
            Debug.Log("Invalid token index");
            return -1;
        }

        // Draw the token at the clicked index
        currentDrawnToken = tokens[index];
        tokens.RemoveAt(index);

        // If all tokens used up, reset the pool
        if (tokens.Count == 0)
        {
            tokens.AddRange(baseTokens);
            Debug.Log($"{color} tokens reset — all 6 used!");
        }

        hasDrawnThisTurn = true;

        Debug.Log($"{color} drew token: {currentDrawnToken} | Remaining: {tokens.Count}");
        return currentDrawnToken;
    }

    // Called when a player lands on a dot
    // Returns true if the bonus draw is allowed
    public bool TryUseDotBonus(PlayerColor color)
    {
        if (hasUsedDotThisTurn)
        {
            Debug.Log("Dot bonus already used this turn — no effect");
            return false;
        }

        hasUsedDotThisTurn = true;
        hasDrawnThisTurn = false; // Allow drawing again
        Debug.Log($"{color} triggered dot bonus — draw another token!");
        return true;
    }

    private bool IsDrawingBonusToken()
    {
        // True if dot bonus was used and we are allowing a second draw
        return hasUsedDotThisTurn && !hasDrawnThisTurn;
    }

    // Call this at the start of each new turn to reset turn-based flags
    public void ResetTurnState()
    {
        currentDrawnToken = -1;
        hasDrawnThisTurn = false;
        hasUsedDotThisTurn = false;
        Debug.Log("Token turn state reset");
    }
}