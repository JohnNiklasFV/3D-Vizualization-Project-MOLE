using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoardField : MonoBehaviour
{
    public int id;

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
        // DRAW PATH CONNECTIONS
        // =========================
        if (BoardManager.Instance == null) return;
        if (!BoardMap.Paths.ContainsKey(id)) return;

        var directions = BoardMap.Paths[id];

        for (int d = 0; d < directions.Length; d++)
        {
            var path = directions[d];
            if (path.Length == 0) continue;

            Color pathColor = Color.HSVToRGB(
                (float)d / directions.Length, 0.8f, 1f
            );
            Gizmos.color = pathColor;

            Vector3 from = transform.position;

            foreach (int targetId in path)
            {
                BoardField target = BoardManager.Instance.GetField(targetId);
                if (target == null) continue;

                Vector3 to = target.transform.position;

                Gizmos.DrawLine(from, to);
                Gizmos.DrawSphere(to, 0.04f);

                from = to; // chain steps in order
            }
        }
    }
#endif
}