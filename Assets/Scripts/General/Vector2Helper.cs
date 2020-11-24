using UnityEngine;
using static GameSettings;

public static class Vector2Helper
{
    //get world position based on row-column coordinates
    public static Vector2 TransformPoint(Vector2Int v)
    {
        float posX = -v.y * CellScale.y;
        float posY = v.x * CellScale.x;

        return new Vector2(posX, posY);
    }

    //get row-column coordinates based on hit.point.xy and current puzzle's row and column count
    //bottom-right: (0, 0); top-left: (ColCount - 1, RowCount - 1)
    public static Vector2Int InverseTransformPoint(Vector2 v)
    {
        v.x *= -1;
        
        int row = Mathf.FloorToInt(v.y * InverseCellScale.y);
        int col = Mathf.FloorToInt(v.x * InverseCellScale.x);

        return new Vector2Int(row, col);
    }

    //return the closest coordinate that lies on the same x or y axis as <start>
    public static Vector2Int GetClosestCoordinate(Vector2Int start, Vector2Int current)
    {
        Vector2Int v = current - start;

        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            v.x = current.x;
            v.y = start.y;
        }
        else
        {
            v.x = start.x;
            v.y = current.y;
        }

        return v;
    }
}