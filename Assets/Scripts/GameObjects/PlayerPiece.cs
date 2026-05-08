using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public PlayerColor playerColor;
    public PieceState state = PieceState.Free;
    public int currentFieldId = -1;

    public bool IsSelectable => state != PieceState.Eliminated;
    public bool IsBurrowed => state == PieceState.Burrowed;
    public bool IsFree => state == PieceState.Free;

    public void MoveTo(BoardField targetField)
    {
        // Clear old field
        if (currentFieldId != -1)
        {
            BoardField oldField = BoardManager.Instance.GetField(currentFieldId);
            if (oldField != null)
                oldField.ClearPiece();
        }

        // Update state based on destination field type
        if (targetField.fieldType == FieldType.Burrow)
            state = PieceState.Burrowed;
        else
            state = PieceState.Free;

        // Move to new field
        currentFieldId = targetField.id;
        targetField.PlacePiece(this);

        // Snap to position
        transform.position = targetField.transform.position;
    }

    public void Eliminate()
    {
        state = PieceState.Eliminated;

        if (currentFieldId != -1)
        {
            BoardField field = BoardManager.Instance.GetField(currentFieldId);
            if (field != null)
                field.ClearPiece();
        }

        currentFieldId = -1;
        gameObject.SetActive(false);
    }
}