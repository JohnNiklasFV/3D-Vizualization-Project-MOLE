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

        int steps = TurnManager.Instance.CurrentSteps;
        TileSelector.Instance.SelectPiece(piece, steps);
    }
}