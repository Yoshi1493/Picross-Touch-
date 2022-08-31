using UnityEngine;

public class InstructionsMenu : Menu
{
    [SerializeField] Canvas[] pages;
    [SerializeField] GameObject previousPageButton, nextPageButton;

    int currentPage;
    int CurrentPage
    {
        get => currentPage;
        set
        {
            currentPage = value;
            ChangePageTo(currentPage);
        }
    }

    public override void Open()
    {
        base.Open();
        CurrentPage = 0;
    }

    public void OnChangePage(int direction)
    {
        CurrentPage = Mathf.Clamp(CurrentPage + direction, 0, pages.Length - 1);
    }

    void ChangePageTo(int pageNumber)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].enabled = i == pageNumber;
        }

        previousPageButton.SetActive(pageNumber != 0);
        nextPageButton.SetActive(pageNumber != pages.Length - 1);
    }
}