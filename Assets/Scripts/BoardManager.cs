using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    public BoardField[] fields;

    private Dictionary<int, BoardField> lookup = new();

    void Awake()
    {
        Instance = this;

        foreach (var field in fields)
        {
            lookup[field.id] = field;
        }
    }

    public BoardField GetField(int id)
    {
        lookup.TryGetValue(id, out var field);
        return field;
    }

    public bool HasField(int id) => lookup.ContainsKey(id);
}
