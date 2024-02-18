using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GlobalShaderProperties))]
public class EditorShaderProperties : Editor
{
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        base.DrawDefaultInspector();

        GlobalShaderProperties selected = target as GlobalShaderProperties;

        if (selected != null)
        {
            selected.UpdateProperties();

            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }

    }
}
