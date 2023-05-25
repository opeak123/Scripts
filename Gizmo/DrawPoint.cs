using UnityEditor;
using UnityEngine;

public class DrawPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 1f);
    }
}
