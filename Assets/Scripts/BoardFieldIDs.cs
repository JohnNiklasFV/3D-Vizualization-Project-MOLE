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
        if (!enabled) return;

        Handles.color = Color.white;

        Vector3 labelPosition = transform.position + Vector3.up * 0.2f;
        Handles.Label(labelPosition, $"ID: {id}");
    }
#endif
}
