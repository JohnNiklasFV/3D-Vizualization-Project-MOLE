using UnityEngine;

public class BoardGridRenderer : MonoBehaviour
{
    public BoardManager boardManager;
    public Color gridColor = Color.yellow;
    public float lineWidth = 0.05f;
    public float heightOffset = 0.05f;

    void Start()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        if (boardManager == null)
        {
            Debug.LogError("BoardManager not assigned!");
            return;
        }

        foreach (BoardField field in boardManager.fields)
        {
            if (!BoardMap.Paths.ContainsKey(field.id))
                continue;

            var directions = BoardMap.Paths[field.id];

            for (int d = 0; d < directions.Length; d++)
            {
                var path = directions[d];
                if (path.Length == 0) continue;

                Vector3 from = field.transform.position;

                foreach (int targetId in path)
                {
                    BoardField target = boardManager.GetField(targetId);
                    if (target == null) continue;

                    Vector3 to = target.transform.position;

                    // Prevent duplicate lines
                    if (field.id < target.id)
                    {
                        CreateLine(from, to);
                    }

                    from = to;
                }
            }
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = transform;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;

        start.y += heightOffset;
        end.y += heightOffset;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.startColor = gridColor;
        lr.endColor = gridColor;
    }
}