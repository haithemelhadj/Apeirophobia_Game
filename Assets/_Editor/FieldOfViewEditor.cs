using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Enemy))]
public class FieldOfViewEditor : MonoBehaviour
{
    /*
    public Enemy enemy;
    public AiAgentTry3 aiAgent;
    //public Transform fovTransform;
    private void OnSceneGUI()
    {
        
        Enemy fov = (Enemy)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.fovTransform.position, Vector3.up, Vector3.forward, 360, fov.radius); // draw white circle

        Vector3 viewAngle01 = DirectionFromAngle(fov.fovTransform.eulerAngles.y, -fov.fovAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.fovTransform.eulerAngles.y, fov.fovAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.fovTransform.position, fov.fovTransform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.fovTransform.position, fov.fovTransform.position + viewAngle02 * fov.radius);

        if (fov.playerInSightRange)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.fovTransform.position, fov.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    */
}
