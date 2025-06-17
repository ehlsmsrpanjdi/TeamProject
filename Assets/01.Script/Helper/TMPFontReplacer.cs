using UnityEditor;
using UnityEngine;
using TMPro;

public class TMPFontReplacer : EditorWindow
{
    TMP_FontAsset newFont;

    [MenuItem("Tools/Replace TMP Font")]
    public static void ShowWindow()
    {
        GetWindow<TMPFontReplacer>("TMP Font Replacer");
    }

    void OnGUI()
    {
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Replace Fonts in Scene"))
        {
            ReplaceFontsInScene();
        }
    }

    void ReplaceFontsInScene()
    {
        if (newFont == null)
        {
            Debug.LogError("No font selected.");
            return;
        }

        TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>(true);
        int count = 0;

        foreach (var tmp in allTexts)
        {
            Undo.RecordObject(tmp, "Replace TMP Font");
            tmp.font = newFont;
            EditorUtility.SetDirty(tmp);
            count++;
        }

        Debug.Log($"Replaced {count} TextMeshProUGUI components.");
    }
}
