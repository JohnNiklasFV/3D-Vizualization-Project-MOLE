using UnityEngine;
using System.Collections.Generic;

public class BoardGridRenderer : MonoBehaviour
{
    public BoardManager boardManager;

    [Header("Grid Settings")]
    public Color gridColor = Color.yellow;
    public float lineWidth = 0.05f;
    public float heightOffset = 0.05f;

    [Header("Dot Settings")]
    public float normalDotSize = 0.08f;
    public float doubleDotSize = 0.18f;
    public Color normalDotColor = Color.yellow;
    public Color doubleDotColor = new Color(1f, 0.5f, 0f); // orange
    public Color highlightDotColor = Color.green;

    // Tracks dot objects per field ID
    private Dictionary<int, GameObject> fieldDots = new();
    private Dictionary<int, bool> isDoubleDot = new();

    public static BoardGridRenderer Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DrawGrid();
        SpawnDots();
    }

    // =========================
    // GRID LINES
    // =========================
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

    // =========================
    // DOTS
    // Spawns a dot at every field position
    // =========================
    void SpawnDots()
    {
        foreach (BoardField field in boardManager.fields)
        {
            GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dot.name = $"Dot_{field.id}";
            dot.transform.parent = transform;

            // Position dot at field with height offset
            Vector3 pos = field.transform.position;
            pos.y += heightOffset;
            dot.transform.position = pos;

            // Start as normal dot size
            float size = normalDotSize;
            dot.transform.localScale = Vector3.one * size;

            // Remove collider — dots are visual only
            Destroy(dot.GetComponent<Collider>());

            // Apply material
            Renderer r = dot.GetComponent<Renderer>();
            Material mat = new Material(Shader.Find("Unlit/Color"));
            mat.color = normalDotColor;
            r.material = mat;

            // Track it
            fieldDots[field.id] = dot;
            isDoubleDot[field.id] = false;
        }
    }

    // =========================
    // UPDATE DOTS FOR LAYER
    // Called by LayerTransitionManager on layer transition
    // =========================
    public void UpdateDotsForLayer(int[] doubleDotIds)
    {
        // Reset all dots to normal
        foreach (var kvp in fieldDots)
        {
            kvp.Value.transform.localScale = Vector3.one * normalDotSize;
            SetDotColor(kvp.Key, normalDotColor);
            isDoubleDot[kvp.Key] = false;
        }

        // Apply double dot size and color
        if (doubleDotIds != null)
        {
            foreach (int id in doubleDotIds)
            {
                if (fieldDots.ContainsKey(id))
                {
                    fieldDots[id].transform.localScale = Vector3.one * doubleDotSize;
                    SetDotColor(id, doubleDotColor);
                    isDoubleDot[id] = true;
                }
            }
        }
    }

    // =========================
    // HIGHLIGHT
    // Called by TileSelector to highlight valid destinations
    // =========================
    public void HighlightField(int fieldId)
    {
        SetDotColor(fieldId, highlightDotColor);
    }

    public void ClearHighlight(int fieldId)
    {
        // Restore original color based on whether it's a double dot
        if (isDoubleDot.ContainsKey(fieldId) && isDoubleDot[fieldId])
            SetDotColor(fieldId, doubleDotColor);
        else
            SetDotColor(fieldId, normalDotColor);
    }

    private void SetDotColor(int fieldId, Color color)
    {
        if (!fieldDots.ContainsKey(fieldId)) return;
        Renderer r = fieldDots[fieldId].GetComponent<Renderer>();
        if (r != null)
            r.material.color = color;
    }
}