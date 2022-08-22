using UnityEngine;
using static GameSettings;

public class Cell : MonoBehaviour
{
    //[0] = filled; [1] = crossed
    SpriteRenderer[] childSprites = new SpriteRenderer[2];

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            childSprites[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    public CellType CurrentCellType { get; private set; }

    //called by InputTool child classes
    public void SetCellType(CellType newCellType)
    {
        if (CurrentCellType == newCellType) return;

        CurrentCellType = newCellType;

        for (int i = 0; i < childSprites.Length; i++)
        {
            switch (CurrentCellType)
            {
                //hide all child sprites
                case CellType.Empty:
                    childSprites[i].enabled = false;
                    break;

                //display filled sprite, hide the others
                case CellType.Filled:
                    childSprites[i].enabled = i == 0;
                    break;

                //display crossed sprite, hide the others
                case CellType.Crossed:
                    childSprites[i].enabled = i == 1;
                    break;
            }
        }
    }
}