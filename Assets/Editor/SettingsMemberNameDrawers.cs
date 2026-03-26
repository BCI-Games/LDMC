using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SettingsFieldNameAttribute))]
public class SettingsFieldNamePropertyDrawer : StringSelectionPropertyDrawer
{
    protected override string[] Options => FieldNameOptions;
    static readonly string[] FieldNameOptions
    = typeof(Settings).GetFields()
    .Select(field => field.Name)
    .Prepend("Unset").ToArray();
}


[CustomPropertyDrawer(typeof(SettingsPropertyNameAttribute))]
public class SettingsPropertyNamePropertyDrawer : StringSelectionPropertyDrawer
{
    protected override string[] Options => PropertyNameOptions;
    static readonly string[] PropertyNameOptions
    = typeof(Settings).GetProperties()
    .Select(field => field.Name)
    .Prepend("Unset").ToArray();
}

public abstract class StringSelectionPropertyDrawer : PropertyDrawer
{
    protected abstract string[] Options { get; }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string value = property.stringValue;

        int selectedIndex = Array.IndexOf(Options, value);
        if (selectedIndex == -1) selectedIndex = 0;

        selectedIndex = EditorGUI.Popup(position, property.displayName, selectedIndex, Options);

        if (selectedIndex > 0)
        {
            property.stringValue = Options[selectedIndex];
        }
    }
}