using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (EnemyVision))]
public class FOVEditor : Editor {

     void OnSceneGUI()
    {
        EnemyVision FOV = (EnemyVision)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(FOV.transform.position, Vector3.up, Vector3.forward, 360, FOV.view_Radius);
        Vector3 viewAngleA = FOV.DirFromAngle(-FOV.view_Angle / 2, false);
        Vector3 viewAngleB = FOV.DirFromAngle(FOV.view_Angle / 2, false);

        Handles.DrawLine(FOV.transform.position, FOV.transform.position + viewAngleA * FOV.view_Radius);
        Handles.DrawLine(FOV.transform.position, FOV.transform.position + viewAngleB * FOV.view_Radius);

        Handles.color = Color.red;
        foreach(Transform visibleTarget in FOV.visibleTargets)
        {
            Handles.DrawLine(FOV.transform.position, visibleTarget.position);
        }

    }
}
