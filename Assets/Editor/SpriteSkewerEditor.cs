using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteSkewer)), CanEditMultipleObjects]
public class SpriteSkewerEditor: Editor
{
    protected virtual void OnSceneGUI()
    {
        SpriteSkewer skewer = (SpriteSkewer)target;

        for (int i = 0; i < 4; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector2 cornerPosition = skewer.GetCornerPosition(i);
            cornerPosition += (Vector2)skewer.transform.position;
            cornerPosition = Handles.PositionHandle(cornerPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(skewer, "Change Position Of Corner " + i);
                cornerPosition -= (Vector2)skewer.transform.position;
                skewer.SetCornerPosition(i, cornerPosition);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        SpriteSkewer skewer = (SpriteSkewer)target;

        if (GUILayout.Button("Reset"))  skewer.ResetCorners();
        base.OnInspectorGUI();
    }
}