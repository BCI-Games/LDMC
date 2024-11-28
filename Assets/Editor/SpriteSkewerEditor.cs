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
            Vector2 handlePosition = skewer.GetHandlePosition(i);
            handlePosition = Handles.PositionHandle(handlePosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(skewer, "Change Position Of Corner " + i);
                skewer.SetCornerPositionFromHandle(i, handlePosition);
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