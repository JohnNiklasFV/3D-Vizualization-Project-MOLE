using UnityEngine;
using System.Collections.Generic;

public class LayerTransitionManager : MonoBehaviour
{
    public static LayerTransitionManager Instance;

    [Header("Layer Visual GameObjects")]
    public GameObject layer1;
    public GameObject layer2;
    public GameObject layer3;
    public GameObject layer4;
    public GameObject layer5;

    [Header("Layer Burrow Parent GameObjects")]
    public GameObject burrows1;
    public GameObject burrows2;
    public GameObject burrows3;
    public GameObject burrows4;

    private int currentLayer = 1;

    // =========================
    // BURROW IDs PER LAYER
    // =========================
    private readonly Dictionary<int, int[]> burrowIdsByLayer = new()
    {
        { 1, new int[] { 2, 4, 18, 19, 21, 24, 31, 38, 41, 48, 50, 51, 58 } },
        { 2, new int[] { 10, 13, 25, 35, 37, 49, 53, 57 } },
        { 3, new int[] { 15, 19, 26, 59 } },
        { 4, new int[] { 31 } }
    };

    // =========================
    // DOT IDs PER LAYER
    // Placeholder — fill in later
    // =========================
    private readonly Dictionary<int, int[]> dotIdsByLayer = new()
    {
        { 1, new int[] { } },
        { 2, new int[] { 61, 22, 28, 1 } },
        { 3, new int[] { 50, 40, 36, 3 } },
        { 4, new int[] { 57, 49, 13, 1 } }
    };

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Apply layer 1 field types on start
        ApplyFieldTypes(1);

        // Make sure only layer 1 is visible at start
        SetActiveLayer(1);
    }

    

    // =========================
    // APPLY FIELD TYPES
    // Sets burrow/dot/normal on all 61 empties for given layer
    // =========================
    private void ApplyFieldTypes(int layer)
    {
        // First reset all fields to Normal
        for (int id = 1; id <= 61; id++)
        {
            BoardField field = BoardManager.Instance.GetField(id);
            if (field != null)
                field.fieldType = FieldType.Normal;
        }

        // Set burrows for this layer
        if (burrowIdsByLayer.ContainsKey(layer))
        {
            foreach (int id in burrowIdsByLayer[layer])
            {
                BoardField field = BoardManager.Instance.GetField(id);
                if (field != null)
                    field.fieldType = FieldType.Burrow;
            }
        }

        // Set dots for this layer (placeholder — empty for now)
        if (dotIdsByLayer.ContainsKey(layer))
        {
            foreach (int id in dotIdsByLayer[layer])
            {
                BoardField field = BoardManager.Instance.GetField(id);
                if (field != null)
                    field.fieldType = FieldType.Dot;
            }
        }

        Debug.Log($"Field types applied for layer {layer}");
    }

    // =========================
    // LAYER VISIBILITY
    // =========================
    private void SetActiveLayer(int layer)
    {
        if (layer1 != null) layer1.SetActive(layer == 1);
        if (layer2 != null) layer2.SetActive(layer == 2);
        if (layer3 != null) layer3.SetActive(layer == 3);
        if (layer4 != null) layer4.SetActive(layer == 4);
        if (layer5 != null) layer5.SetActive(false); // win screen only
        Debug.Log($"Layer {layer} is now visible");
    }

    // =========================
    // TRANSITION TO NEXT LAYER
    // Called by TurnManager when all burrows are filled
    // =========================
    public void TransitionToNextLayer()
    {
        if (currentLayer >= 4)
        {
            Debug.Log("Already on final layer!");
            return;
        }

        Debug.Log($"Transitioning from layer {currentLayer} to layer {currentLayer + 1}");

        // Eliminate pieces that are not burrowed
        EliminateNonBurrowedPieces();

        currentLayer++;

        // Swap visual layer
        SetActiveLayer(currentLayer);

        // Update field types for new layer
        ApplyFieldTypes(currentLayer);

        // Update dot visuals for new layer
        if (BoardGridRenderer.Instance != null)
            BoardGridRenderer.Instance.UpdateDotsForLayer(dotIdsByLayer[currentLayer]);

        // Reposition burrowed pieces to their new field positions
        RepositionBurrowedPieces();

        Debug.Log($"Now on layer {currentLayer}");

        // Notify TurnManager
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnLayerTransition(currentLayer);
    }

    // =========================
    // ELIMINATION
    // Removes all pieces not in a burrow
    // =========================
    private void EliminateNonBurrowedPieces()
    {
        // Get all pieces in scene
        PlayerPiece[] allPieces = FindObjectsByType<PlayerPiece>(FindObjectsSortMode.None);

        int eliminated = 0;
        foreach (PlayerPiece piece in allPieces)
        {
            if (piece.state != PieceState.Burrowed)
            {
                piece.Eliminate();
                eliminated++;
            }
        }

        Debug.Log($"{eliminated} pieces eliminated");
    }

    // =========================
    // REPOSITION BURROWED PIECES
    // After layer swap, snap burrowed pieces to their field positions
    // Since GridNodes stay in place, positions are unchanged
    // =========================
    private void RepositionBurrowedPieces()
    {
        PlayerPiece[] allPieces = FindObjectsByType<PlayerPiece>(FindObjectsSortMode.None);

        foreach (PlayerPiece piece in allPieces)
        {
            if (piece.state == PieceState.Burrowed)
            {
                BoardField field = BoardManager.Instance.GetField(piece.currentFieldId);
                if (field != null)
                {
                    piece.transform.position = field.transform.position;
                    // After transition burrowed pieces become free again
                    piece.state = PieceState.Free;
                    Debug.Log($"Piece repositioned to field {field.id} — now Free");
                }
            }
        }
    }

    // =========================
    // BURROW CHECK
    // Called after every move to check if all burrows are filled
    // =========================
    public bool AllBurrowsFilled()
    {
        if (!burrowIdsByLayer.ContainsKey(currentLayer))
            return false;

        foreach (int id in burrowIdsByLayer[currentLayer])
        {
            BoardField field = BoardManager.Instance.GetField(id);
            if (field == null) return false;
            if (!field.IsOccupied) return false;
        }

        return true;
    }

    public int CurrentLayer => currentLayer;
}