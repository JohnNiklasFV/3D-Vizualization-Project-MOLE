using UnityEngine;
using System.Collections.Generic;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;

    [Header("Piece Prefabs")]
    public GameObject piecePrefabRed;
    public GameObject piecePrefabBlue;
    public GameObject piecePrefabGreen;
    public GameObject piecePrefabYellow;

    [Header("Placement Settings")]
    public int playerCount = 2;

    // Tracks how many pieces each player has left to place
    private Dictionary<PlayerColor, int> piecesLeftToPlace = new();

    // Tracks all spawned pieces per player
    private Dictionary<PlayerColor, List<PlayerPiece>> spawnedPieces = new();

    // Player order for placement
    private List<PlayerColor> playerOrder = new();
    private int currentPlayerIndex = 0;

    private bool isPlacementPhase = true;

    public bool IsPlacementPhase => isPlacementPhase;
    public PlayerColor CurrentPlacingPlayer => playerOrder[currentPlayerIndex];

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Read player count from main menu selection
        // Defaults to 2 if PlayerPrefs not set
        playerCount = PlayerPrefs.GetInt("PlayerCount", 2);
        Debug.Log($"Player count loaded: {playerCount}");

        InitializePlacement();
    }

    private void InitializePlacement()
    {
        // Hide token UI during placement
        if (TokenUIManager.Instance != null)
            TokenUIManager.Instance.gameObject.SetActive(false);

        // Build player order based on saved color choices
        playerOrder.Clear();

        for (int i = 1; i <= playerCount; i++)
        {
            // Read each player's color from PlayerPrefs
            // Default order: Red, Blue, Green, Yellow if not set
            int colorIndex = PlayerPrefs.GetInt($"Player{i}Color", i - 1);
            PlayerColor color = (PlayerColor)colorIndex;
            playerOrder.Add(color);
            Debug.Log($"Player {i} color: {color}");
        }

        // Calculate pieces per player
        int piecesPerPlayer = GetPiecesPerPlayer(playerCount);

        // Initialize tracking dictionaries
        foreach (PlayerColor color in playerOrder)
        {
            piecesLeftToPlace[color] = piecesPerPlayer;
            spawnedPieces[color] = new List<PlayerPiece>();
        }

        Debug.Log($"Placement phase started — {playerCount} players, {piecesPerPlayer} pieces each");
        Debug.Log($"{CurrentPlacingPlayer}'s turn to place a piece");

        if (TurnIndicatorUI.Instance != null)
            TurnIndicatorUI.Instance.ShowPlacementText(
                CurrentPlacingPlayer,
                piecesLeftToPlace[CurrentPlacingPlayer]
            );
    }

    private int GetPiecesPerPlayer(int count)
    {
        switch (count)
        {
            case 2: return 10;
            case 3: return 7;
            case 4: return 6;
            default: return 10;
        }
    }

    // Called by BoardField when clicked during placement phase
    public void TryPlacePiece(BoardField field)
    {
        if (!isPlacementPhase) return;

        // Cannot place on occupied field
        if (field.IsOccupied)
        {
            Debug.Log("Field is already occupied!");
            return;
        }

        // Cannot place in a burrow
        if (field.fieldType == FieldType.Burrow)
        {
            Debug.Log("Cannot place a piece in a burrow at the start!");
            return;
        }

        // Cannot place on blocked field
        if (field.fieldType == FieldType.Blocked)
        {
            Debug.Log("Cannot place on a blocked field!");
            return;
        }

        // Spawn the piece
        PlayerPiece piece = SpawnPiece(CurrentPlacingPlayer, field);
        if (piece == null) return;

        // Track it
        spawnedPieces[CurrentPlacingPlayer].Add(piece);
        piecesLeftToPlace[CurrentPlacingPlayer]--;

        Debug.Log($"{CurrentPlacingPlayer} placed a piece on field {field.id} — {piecesLeftToPlace[CurrentPlacingPlayer]} left to place");

        // Check if all pieces are placed
        if (AllPiecesPlaced())
        {
            StartGame();
            return;
        }

        // Pass to next player
        currentPlayerIndex = (currentPlayerIndex + 1) % playerOrder.Count;

        // Skip players who have placed all their pieces
        int safetyCounter = 0;
        while (piecesLeftToPlace[CurrentPlacingPlayer] == 0)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % playerOrder.Count;
            safetyCounter++;
            if (safetyCounter > playerOrder.Count) break;
        }

        
        if (TurnIndicatorUI.Instance != null)
        {
            if (!AllPiecesPlaced())
                TurnIndicatorUI.Instance.ShowPlacementText(
                    CurrentPlacingPlayer,
                    piecesLeftToPlace[CurrentPlacingPlayer]
                );
        }

        Debug.Log($"{CurrentPlacingPlayer}'s turn to place a piece — {piecesLeftToPlace[CurrentPlacingPlayer]} remaining");
    }

    private PlayerPiece SpawnPiece(PlayerColor color, BoardField field)
    {
        GameObject prefab = GetPrefabForColor(color);
        if (prefab == null)
        {
            Debug.Log($"No prefab assigned for {color}!");
            return null;
        }

        // Spawn at field position
        GameObject pieceObj = Instantiate(prefab, field.transform.position, Quaternion.identity);
        PlayerPiece piece = pieceObj.GetComponent<PlayerPiece>();

        if (piece == null)
        {
            Debug.Log("Spawned object has no PlayerPiece component!");
            Destroy(pieceObj);
            return null;
        }

        // Set up piece
        piece.playerColor = color;
        piece.currentFieldId = field.id;
        piece.state = PieceState.Free;

        // Register on field
        field.PlacePiece(piece);

        return piece;
    }

    private GameObject GetPrefabForColor(PlayerColor color)
    {
        switch (color)
        {
            case PlayerColor.Red: return piecePrefabRed;
            case PlayerColor.Blue: return piecePrefabBlue;
            case PlayerColor.Green: return piecePrefabGreen;
            case PlayerColor.Yellow: return piecePrefabYellow;
            default: return null;
        }
    }

    private bool AllPiecesPlaced()
    {
        foreach (PlayerColor color in playerOrder)
        {
            if (piecesLeftToPlace[color] > 0)
                return false;
        }
        return true;
    }

    private void StartGame()
    {
        isPlacementPhase = false;
        Debug.Log("All pieces placed — starting game!");

        // Show token UI
        if (TokenUIManager.Instance != null)
            TokenUIManager.Instance.gameObject.SetActive(true);

        // Pass spawned pieces to TurnManager
        if (TurnManager.Instance != null)
            TurnManager.Instance.StartGame(spawnedPieces, playerCount);
    }

    // Returns all pieces for a specific player
    public List<PlayerPiece> GetPiecesForPlayer(PlayerColor color)
    {
        if (spawnedPieces.ContainsKey(color))
            return spawnedPieces[color];
        return new List<PlayerPiece>();
    }
}