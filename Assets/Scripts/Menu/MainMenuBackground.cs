using System.Collections;
using UnityEngine;
using static CoroutineHelper;

public class MainMenuBackground : MonoBehaviour
{
    GridLine[] gridLines;

    void Awake()
    {
        gridLines = GetComponentsInChildren<GridLine>();

        //shuffle array order
        for (int i = 0; i < gridLines.Length; i++)
        {
            int r = Random.Range(i, gridLines.Length);
            GridLine temp = gridLines[i];
            gridLines[i] = gridLines[r];
            gridLines[r] = temp;
        }
    }

    IEnumerator Start()
    {
        #region DEBUG
        //float camWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;
        //GameObject.Find("Title").GetComponent<TMPro.TextMeshProUGUI>().text = camWidth.ToString();
        #endregion

        foreach (GridLine g in gridLines)
        {
            g.enabled = true;
            yield return WaitForSeconds(0.05f);
        }
    }
}