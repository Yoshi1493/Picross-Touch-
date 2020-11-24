using UnityEngine;
using static GameSettings;

public static class LevelParser
{
    public static Picross ConvertToPicross(this TextAsset levelFile)
    {
        //split file by row
        string[] rows = levelFile.text.Split('\n');
        CellType[,] cells = new CellType[rows.Length, rows[0].Split(' ').Length];

        for (int i = 0; i < cells.GetLength(0); i++)
        {
            //split line by space
            string[] cols = rows[i].Split(' ');

            //add each char as an int into respective row
            for (int j = 0; j < cells.GetLength(0); j++)
            {
                cells[cells.GetLength(0) - 1 - i, cells.GetLength(0) - 1 - j] = (CellType)int.Parse(cols[j]);
            }
        }

        //get level name
        string fileName = levelFile.name;
        string levelName = fileName.Substring(fileName.IndexOf(']') + 2);

        return new Picross(levelName, cells);
    }
}