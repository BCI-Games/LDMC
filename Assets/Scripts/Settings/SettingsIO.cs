using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static partial class Settings
{
    const string FileName = "settings.json";
    private static string FilePath => Application.dataPath + "/../" + FileName;


    public static void LoadAndApplySettings()
    {
        LoadSettings();
        ConnectModificationEvents();
        WriteSettingsAndNotify();
    }


    private static void LoadSettings()
    {
        StreamReader reader = new(FilePath);
        string fileContent = reader.ReadToEnd();
        reader.Close();

        string bodyString = fileContent.Trim(new char[] { '{', '}' });
        foreach (string fieldString in bodyString.Split(','))
        {
            string[] parts = fieldString.Split(':');
            string fieldName = parts[0].Trim().Trim('"');

            if (GetSetting(fieldName, out ValueProxy field))
            {
                field.SetValue(parts[1]);
            }
        }
    }

    private static void WriteSettings()
    {
        FieldInfo[] fields = typeof(Settings).GetFields();

        static string GetFieldString(FieldInfo field)
        {
            var attributes = field.GetCustomAttributes();
            string prefix
                = attributes.Any(a => a is SpaceAttribute)
                ? "\n\t" : "\t";
            return $"{prefix}\"{field.Name}\": {field.GetValue(null)}";
        }

        string bodyString = string.Join(",\n", fields.Select(GetFieldString));
        string fileContent = $"{{\n{bodyString}\n}}";

        StreamWriter writer = new(FilePath);
        writer.Write(fileContent);
        writer.Close();
    }


    private static void WriteSettingsAndNotify()
    {
        WriteSettings();
        Modified?.Invoke();
    }

    private static void ConnectModificationEvents()
    {
        FieldInfo[] fields = typeof(Settings).GetFields();
        foreach (FieldInfo field in fields)
        {
            if (field.GetValue(null) is ValueProxy proxy)
            {
                proxy.Modified += WriteSettingsAndNotify;
            }
        }
    }
}