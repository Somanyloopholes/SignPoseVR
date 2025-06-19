using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PoseSelectorRuntime))]
public class PoseSelectorRuntimeEditor : Editor
{
    SerializedProperty _libraryProp;
    SerializedProperty _indexProp;

    void OnEnable()
    {
        _libraryProp = serializedObject.FindProperty("poseLibrary");
        _indexProp   = serializedObject.FindProperty("poseIndex");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the library field first
        EditorGUILayout.PropertyField(_libraryProp);

        // If library assigned, make a dropdown of its entries
        var lib = _libraryProp.objectReferenceValue as PoseLibrary;
        if (lib != null && lib.poses != null && lib.poses.Length > 0)
        {
            // Build name list, fall back to type name if null
            string[] names = new string[lib.poses.Length];
            for (int i = 0; i < names.Length; i++)
                names[i] = lib.poses[i]?.name ?? $"<empty {i}>";

            // Dropdown
            int newIndex = EditorGUILayout.Popup(
                new GUIContent("Pose To Detect", 
                    "Select which pose from the library to activate"),
                _indexProp.intValue,
                names
            );

            if (newIndex != _indexProp.intValue)
            {
                _indexProp.intValue = newIndex;
            }
        }
        else
        {
            // Fallback to int field if no library or empty
            EditorGUILayout.PropertyField(_indexProp);
        }

        // Draw the rest of the default inspector (if you have other props)
        DrawPropertiesExcluding(serializedObject, "poseLibrary", "poseIndex");

        serializedObject.ApplyModifiedProperties();
    }
}
