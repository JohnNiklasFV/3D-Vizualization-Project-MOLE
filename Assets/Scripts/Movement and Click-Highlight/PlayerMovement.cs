using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerPiece piece;

    void Awake()
    {
        piece = GetComponent<PlayerPiece>();
    }

    void OnMouseDown()
    {
        Debug.Log($"Piece OnMouseDown fired on {gameObject.name}");
        if (piece == null) return;
        if (!piece.IsSelectable) return;

        // Hardcoded for testing — change this number to test different step counts
        int testSteps = 3;
        TileSelector.Instance.SelectPiece(piece, testSteps);
    }
}