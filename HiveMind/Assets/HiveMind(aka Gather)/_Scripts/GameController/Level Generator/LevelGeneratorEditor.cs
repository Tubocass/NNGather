using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(NewLevelGenerator))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NewLevelGenerator myScript = (NewLevelGenerator)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.Init();
        }
    }
}