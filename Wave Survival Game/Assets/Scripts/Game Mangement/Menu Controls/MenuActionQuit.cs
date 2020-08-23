using UnityEngine;

public class MenuActionQuit : MonoBehaviour, IMenuAction
{
    /// <summary>
    /// Quit game on click
    /// </summary>
    public void clickAction()
    {
        Application.Quit();
    }
}
