
using SplineMesh;
using UnityEngine;

public class FinishLane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var grandParent = other.transform.parent.parent;
        if (grandParent != null)
        {
            if (grandParent.TryGetComponent(out Spline script))
            {
                Debug.Log("Game Ended!");
                UIManager.Instance.LevelEnd();
            }
        }
            
    }
}
