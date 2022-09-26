using System.IO;
using UnityEditor;
using UnityEngine;
using static GameSettings;

public class LevelEditor : MonoBehaviour
{
#if UNITY_EDITOR
    public void LoadLevel()
    {
        var path = EditorUtility.OpenFilePanel(
            "Load level",
            $"{Application.dataPath}/Scripts/Game/Level Files/{targetPuzzleData.RowCount}x{targetPuzzleData.ColCount}",
            "txt"
            );

        if (path.Length != 0)
        {
            var str = File.ReadAllText(path);

            char[] delims = new[] { ' ', '\r', '\n' };
            string[] str1D = str.Split(delims, System.StringSplitOptions.RemoveEmptyEntries);
            string[,] str2D = new string[targetPuzzleData.RowCount, targetPuzzleData.ColCount];

            for (int r = 0; r < str2D.GetLength(0); r++)
            {
                for (int c = 0; c < str2D.GetLength(1); c++)
                {
                    str2D[r, c] = str1D[r * targetPuzzleData.ColCount + c];
                }
            }

            FindObjectOfType<Game>().LoadDataToCurrentLevel(str2D);
        }

    }

    public void SaveLevel()
    {
        var path = EditorUtility.SaveFilePanel(
            "Save level",
            $"{Application.dataPath}/Scripts/Game/Level Files/{targetPuzzleData.RowCount}x{targetPuzzleData.ColCount}",
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