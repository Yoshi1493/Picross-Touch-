using UnityEngine;

public class InstructionsMenu : Menu
{
    [SerializeField] GameObject[] instructionPages;
    int currentPage;

    //0 = prev. page; 1 = next page
    [SerializeField] GameObject[] pageChangeButtons = new GameObject[2];

    public void OnChangePage(int amount)
    {
        //update current instruction page
        instructionPages[currentPage].SetActive(false);
        currentPage = Mathf.Clamp(currentPage + amount, 0, instructionPages.Length - 1);
        instructionPages[currentPage].SetActive(true);

        //update page change buttons based on currentPage
        pageChangeButtons[0].SetActive(currentPage > 0);
        pageChangeButtons[1].SetActive(currentPage < instructionPages.Length - 1);
    }
}