using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionReloadScene : MonoBehaviour, IMenuAction
{
    /// <summary>
    /// Reloads active scene when clicked
    /// </summary>
    public void clickAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().path);
    }
}
