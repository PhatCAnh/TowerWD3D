using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(C_Tower))]
public class EC_Tower : Editor
{
    public override void OnInspectorGUI()
    {
        C_Tower tower = target as C_Tower;
        if (GUILayout.Button("Level_Up"))
        {
            tower.Level_Up();
        }
    }

}
