using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionReturnMM : MonoBehaviour, IMenuAction
{
    /// <summary>
    /// Constant Scene Path
    /// </summary>
    private static string mainMenuPath = "Assets/Scenes/Main Menu.unity";


    /// <summary>
    /// Loads main menu when clicked
    /// </summary>
    public void clickAction()
    {
        SceneManager.LoadScene(mainMenuPath);
    }
}
