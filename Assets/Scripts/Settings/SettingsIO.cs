using UnityEngine;
using System.IO;
using System.Reflection;
using System.Linq;

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
        foreach(string fieldString in bodyString.Split(','))
        {
            string[] parts = fieldString.Split(':');
            string fieldName = parts[0].Trim().Trim('"');

            if (GetField(fieldName, out ValueProxy field))
            {
                field.SetValue(parts[1]);
            }
        }
    }

    private static void WriteSettings()
    {
        FieldInfo[] fields = typeof(Settings).GetFields();

        string bodyString = string.Join(
            ",\n", fields.Select(
                field => $"\t\"{field.Name}\": {field.GetValue(null)}"
            )
        );
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