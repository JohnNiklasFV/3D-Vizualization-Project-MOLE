using UnityEngine;

public class BoardMapTester : MonoBehaviour
{
    void Start()
    {
        TestMove(1, 0, 1); // From field 1, direction 0, 1 step
        TestMove(1, 0, 4); // From field 1, direction 0, 4 steps
        TestMove(1, 2, 3); // From field 1, direction 2, 3 steps
    }

    void TestMove(int fieldId, int direction, int steps)
    {
        var paths = BoardMap.Paths[fieldId];
        int target = paths[direction][steps - 1];

        Debug.Log(
            $"FROM {fieldId} | DIR {direction} | STEPS {steps} → FIELD {target}"
        );
    }
}