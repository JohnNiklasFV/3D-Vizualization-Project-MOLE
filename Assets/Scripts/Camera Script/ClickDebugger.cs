using UnityEngine;

public class ClickDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse click detected");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Hit: {hit.collider.gameObject.name}");
            }
            else
            {
                Debug.Log("Raycast hit nothing");
            }
        }
    }
}