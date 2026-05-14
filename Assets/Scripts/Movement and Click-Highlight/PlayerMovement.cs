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

        if (!TurnManager.Instance.IsCurrentPlayer(piece.playerColor))
        {
            Debug.Log("Not your turn!");
            return;
        }

        if (!TurnManager.Instance.CanSelectPiece())
        {
            Debug.Log("Draw a token first!");
            return;
        }

        // If dot bonus is active, only allow the dot piece to move
        if (TurnManager.Instance.DotBonusPiece != null && 
            TurnManager.Instance.DotBonusPiece != piece)
        {
            Debug.Log("You must move the piece that landed on the dot!");
            return;
        }

        int steps = TurnManager.Instance.CurrentSteps;
        TileSelector.Instance.SelectPiece(piece, steps);
    }
}