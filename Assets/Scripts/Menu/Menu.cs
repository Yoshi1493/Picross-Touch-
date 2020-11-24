using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Menu : MonoBehaviour
{
    protected Canvas thisMenu;

    protected virtual void Awake()
    {
        thisMenu = GetComponent<Canvas>();
    }

    public void Open()
    {
        thisMenu.enabled = true;
        if (thisMenu.TryGetComponent(out Menu m)) m.enabled = true;
    }

    public virtual void Close()
    {
        thisMenu.enabled = false;
        enabled = false;
    }

    public void Disable()
    {
        if (TryGetComponent(out CanvasGroup cg))
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void Enable()
    {
        if (TryGetComponent(out CanvasGroup cg))
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}