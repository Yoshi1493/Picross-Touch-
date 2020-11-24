using System.IO;
using UnityEditor;
using UnityEngine;
using static GameSettings;

public class LevelEditor : MonoBehaviour
{
#if UNITY_EDITOR
    public void SaveLevel()
    {
        var path = EditorUtility.SaveFilePanel(
            "Save level",
            "D:/Program Files/Unity/Projects/Momograms/Assets/Scripts/Game/Level Files",
            $"[{targetPuzzleData.RowCount}x{targetPuzzleData.ColCount}] .txt",
            "txt"
            );

        if (path.Length != 0)
        {
            File.WriteAllText(path, FindObjectOfType<Game>().ConvertCurrentLevelToData());
        }
    }
#endif
}