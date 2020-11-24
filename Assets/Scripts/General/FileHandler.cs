using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static GameSettings;

public static class FileHandler
{
    #region Player settings

    static readonly string settingsDirectoryPath = Path.Combine(Application.persistentDataPath, "Settings");
    static readonly string settingsFilePath = Path.Combine(settingsDirectoryPath, "settings.json");

    public static void SaveSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var file = File.OpenWrite(settingsFilePath))
        {
            bf.Serialize(file, playerSettings);
        }
    }

    public static PlayerSettings LoadSettings()
    {
        Directory.CreateDirectory(settingsDirectoryPath);
        PlayerSettings ps;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var file = File.Open(settingsFilePath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                ps = bf.Deserialize(file) as PlayerSettings;
            }
        }
        catch (SerializationException)
        {
            ps = new PlayerSettings();
        }

        return ps;
    }

    #endregion

    #region Puzzle data files

    static readonly string saveDataDirectoryPath = Path.Combine(Application.persistentDataPath, "Save Data");
    static readonly string saveDataFilePath = Path.Combine(saveDataDirectoryPath, "data.json");

    public static void SavePuzzles()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var file = File.Open(saveDataFilePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            bf.Serialize(file, puzzles);
        }
    }

    public static void LoadPuzzles()
    {
        Directory.CreateDirectory(saveDataDirectoryPath);

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var file = File.OpenRead(saveDataFilePath))
            {
                puzzles = bf.Deserialize(file) as List<Picross>[];
            }
        }
        catch (FileNotFoundException)
        {
            puzzles = new List<Picross>[DifficultyCount];
            for (int i = 0; i < DifficultyCount; i++)
            {
                puzzles[i] = new List<Picross>();
            }
        }
    }

    #endregion

    #region Puzzle image files

    //convert puzzle data into pixel colours, and save it as a jpg file
    public static void SaveAsCompletedImage(this Picross puzzle)
    {
        //set image dimensions
        int imageWidth = puzzle.ColCount;
        int imageHeight = puzzle.RowCount;

        //set directory and file path based on puzzle properties
        string imageDirectoryPath = Path.Combine(saveDataDirectoryPath, $"{imageWidth}x{imageHeight}");
        string imageFilePath = Path.Combine(imageDirectoryPath, $"{puzzle.name}.json");

        //continue only if file doesn't exist yet
        if (File.Exists(imageFilePath)) return;

        Directory.CreateDirectory(imageDirectoryPath);

        Texture2D tex2D = new Texture2D(imageWidth, imageHeight);
        Color[] pixelColours = new Color[imageWidth * imageHeight];

        //set pixel colours
        for (int row = 0; row < imageHeight; row++)
        {
            for (int col = 0; col < imageWidth; col++)
            {
                pixelColours[row * imageWidth + col] = puzzle.cellData.Cells[row, imageWidth - 1 - col] == CellType.Filled ? Color.black : Color.white;
            }
        }

        tex2D.SetPixels(pixelColours);
        tex2D.Apply();

        byte[] bytes = tex2D.EncodeToPNG();

        BinaryFormatter bf = new BinaryFormatter();
        using (var file = File.Open(imageFilePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            bf.Serialize(file, bytes);
        }
    }

    public static byte[] LoadCompletedImage(Picross puzzle)
    {
        string imageFilePath = Path.Combine(saveDataDirectoryPath, $"{puzzle.RowCount}x{puzzle.ColCount}", $"{puzzle.name}.json");

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var file = File.OpenRead(imageFilePath))
            {
                return bf.Deserialize(file) as byte[];
            }
        }
        catch (System.Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException)
        {
            return null;
        }
    }

    #endregion
}