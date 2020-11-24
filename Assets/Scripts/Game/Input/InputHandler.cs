using System.Collections.Generic;
using UnityEngine;
using static Vector2Helper;

public class InputHandler : MonoBehaviour
{
    Camera mainCam;
    Game gameController;

    static readonly Vector2Int outOfBounds = new Vector2Int(-1, -1);
    Vector2Int startCell = outOfBounds;
    Vector2Int currentCell = outOfBounds;
    Vector2Int closestCell = outOfBounds;

    List<Vector2Int> selectedCells = new List<Vector2Int>();

    public event System.Action<Vector2Int, Vector2Int> HighlightStartAction;
    public event System.Action<List<Vector2Int>> HighlightEndAction;

    void Awake()
    {
        mainCam = Camera.main;
        gameController = GetComponent<Game>();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        //on click down
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Collider2D coll = hit.collider;

            if (coll) startCell = InverseTransformPoint(hit.point);
        }

        //on click drag
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Collider2D coll = hit.collider;

            //make sure player actually has something selected
            if (coll && !startCell.Equals(outOfBounds))
            {
                //get position of where the player clicked
                Vector2 _currentCell = InverseTransformPoint(hit.point);

                if (!_currentCell.Equals(currentCell))
                {
                    //update currentCell only on the frame that _currentCell changes
                    currentCell = InverseTransformPoint(hit.point);
                    _currentCell = currentCell;

                    //find the cell closest to currentCell
                    Vector2 _closestCell = GetClosestCoordinate(startCell, currentCell);

                    if (!_closestCell.Equals(closestCell))
                    {
                        //update closestCell only on the frame that _closestCell changes
                        closestCell = GetClosestCoordinate(startCell, currentCell);
                        _closestCell = closestCell;

                    }

                    HighlightStartAction?.Invoke(startCell, closestCell);
                }
            }
        }

        //on click up
        if (Input.GetMouseButtonUp(0))
        {
            //make sure player actually has something selected
            if (!startCell.Equals(outOfBounds))
            {
                Vector2Int difference = closestCell - startCell;
                Vector2Int clampedDirection = new Vector2Int(
                    Mathf.Clamp(difference.x, -1, 1),
                    Mathf.Clamp(difference.y, -1, 1)
                    );
                float magnitude = difference.magnitude;

                //add all cells from startCell to closestCell to list of cells to select
                for (int i = 0; i <= magnitude; i++)
                {
                    Vector2Int offset = new Vector2Int(clampedDirection.x * i, clampedDirection.y * i);
                    selectedCells.Add(startCell + offset);
                }

                HighlightEndAction?.Invoke(selectedCells);

                //reset vars
                startCell = outOfBounds;
                currentCell = outOfBounds;
                selectedCells.Clear();
            }
        }
    }
}