using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoardField : MonoBehaviour
{
    // =========================
    // FIELD IDENTITY
    // =========================
    // The unique ID that links this GameObject to the BoardMap paths
    public int id;

    // What type of field this is — set this in the Inspector for each field
    // Normal = regular walkable space
    // Burrow = a hole a mole can enter
    // Dot = triggers bonus token draw if landed on
    // Blocked = cannot be walked through (used on final layer)
    public FieldType fieldType = FieldType.Normal;

    // =========================
    // OCCUPANCY TRACKING
    // =========================
    // The piece currently standing on this field, null if empty
    public PlayerPiece occupiedBy = null;

    // Quick check — is there a piece on this field?
    public bool IsOccupied => occupiedBy != null;

    // Call this when a piece arrives on this field
    public void PlacePiece(PlayerPiece piece)
    {
        occupiedBy = piece;
    }

    // Call this when a piece leaves this field
    public void ClearPiece()
    {
        occupiedBy = null;
    }

    // =========================
    // CLICK DETECTION
    // =========================
    // When a highlighted field is clicked, tell TileSelector to confirm the move
    // Requires a Collider on this GameObject to work
    void OnMouseDown()
    {
        // During placement phase — place a piece
        if (PlacementManager.Instance != null && 
            PlacementManager.Instance.IsPlacementPhase)
        {
            PlacementManager.Instance.TryPlacePiece(this);
            return;
        }

        // During game phase — select destination
        TileSelector.Instance.SelectDestination(this);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // =========================
        // DRAW FIELD ID LABEL
        // =========================
        Vector3 labelPosition = transform.position + Vector3.up * 0.25f;
        Handles.color = Color.white;
        Handles.Label(labelPosition, $"ID: {id}");

        // =========================
        // DRAW FIELD TYPE LABEL
        // =========================
        // Shows the field type (Normal, Burrow etc.) just above the ID in the scene view
        Vector3 typePosition = transform.position + Vector3.up * 0.45f;
        Handles.color = Color.cyan;
        Handles.Label(typePosition, fieldType.ToString());

        // =========================
        // DRAW PATH CONNECTIONS
        // =========================
        // Draws colored lines in the scene view showing every possible path
        // from this field — each direction gets its own color
        if (BoardManager.Instance == null) return;
        if (!BoardMap.Paths.ContainsKey(id)) return;

        var directions = BoardMap.Paths[id];

        for (int d = 0; d < directions.Length; d++)
        {
            var path = directions[d];
            if (path.Length == 0) continue;

            // Each direction gets a unique color based on its index
            Color pathColor = Color.HSVToRGB(
                (float)d / directions.Length, 0.8f, 1f
            );
            Gizmos.color = pathColor;

            Vector3 from = transform.position;

            // Draw a line from each step to the next, chaining them together
            foreach (int targetId in path)
            {
                BoardField target = BoardManager.Instance.GetField(targetId);
                if (target == null) continue;

                Vector3 to = target.transform.position;

                Gizmos.DrawLine(from, to);
                Gizmos.DrawSphere(to, 0.04f);

                from = to;
            }
        }
    }
#endif
}