using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class TileSelector : MonoBehaviour
{
    public static TileSelector Instance;

    [Header("Highlight Materials")]
    public Material highlightMaterial;
    public Material defaultMaterial;

    private List<BoardField> highlightedFields = new();
    private PlayerPiece selectedPiece = null;
    private bool isProcessingMove = false;

    void Awake()
    {
        Instance = this;
    }

    // Called when a player clicks a piece
    public void SelectPiece(PlayerPiece piece, int steps)
    {
        if (isProcessingMove) return;
        ClearHighlights();
        selectedPiece = piece;

        List<BoardField> validDestinations = GetValidDestinations(piece, steps);
        Debug.Log($"Valid destinations found: {validDestinations.Count}");

        if (validDestinations.Count == 0)
        {
            Debug.Log($"No valid moves for piece on field {piece.currentFieldId}");
            selectedPiece = null;
            return;
        }

        foreach (BoardField field in validDestinations)
        {
            HighlightField(field);
        }
    }

    // Called when a player clicks a highlighted field
    public void SelectDestination(BoardField field)
    
    {
        if (selectedPiece == null) return;
        if (!highlightedFields.Contains(field)) return;

        StartCoroutine(ProcessMove(field, selectedPiece));
        selectedPiece = null;
    }

    private IEnumerator ProcessMove(BoardField field, PlayerPiece piece)
    {
        isProcessingMove = true;

        ClearHighlights();
        piece.MoveTo(field);

        if (TurnManager.Instance != null)
            TurnManager.Instance.OnMoveMade(field);

        yield return new WaitForSeconds(0.1f);
        isProcessingMove = false;
    }

    public List<BoardField> GetValidDestinations(PlayerPiece piece, int steps)
    {
        List<BoardField> valid = new();

        if (!BoardMap.Paths.ContainsKey(piece.currentFieldId))
            return valid;

        var directions = BoardMap.Paths[piece.currentFieldId];

        foreach (var path in directions)
        {
            // Path must have enough steps
            if (path.Length < steps) continue;

            // Check every field along the way for blockers
            bool blocked = false;
            for (int i = 0; i < steps; i++)
            {
                BoardField stepField = BoardManager.Instance.GetField(path[i]);

                if (stepField == null) continue;

                // Blocked field type is never passable
                if (stepField.fieldType == FieldType.Blocked)
                {
                    blocked = true;
                    break;
                }

                // Any occupied field blocks the path
                if (stepField.IsOccupied)
                {
                    blocked = true;
                    break;
                }
            }

            if (blocked) continue;

            // Destination is the field at index steps - 1
            BoardField destination = BoardManager.Instance.GetField(path[steps - 1]);
            if (destination != null && !valid.Contains(destination))
                valid.Add(destination);
        }

        return valid;
    }

    private Dictionary<BoardField, Material> originalMaterials = new();

    private void HighlightField(BoardField field)
    {
        highlightedFields.Add(field);

        Renderer r = field.GetComponent<Renderer>();
        if (r != null && highlightMaterial != null)
        {
            originalMaterials[field] = r.material;
            r.material = highlightMaterial;
        }
    }

    public void ClearHighlights()
    {
        foreach (BoardField field in highlightedFields)
        {
            Renderer r = field.GetComponent<Renderer>();
            if (r != null && originalMaterials.ContainsKey(field))
                r.material = originalMaterials[field];
        }

        highlightedFields.Clear();
        originalMaterials.Clear();
        selectedPiece = null;
    }
}