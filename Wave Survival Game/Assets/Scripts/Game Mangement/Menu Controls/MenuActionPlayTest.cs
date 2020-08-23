using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionPlayTest : MonoBehaviour, IMenuAction
{
    /// <summary>
    ///     Constant Scene Path 
    /// </summary>
    private const string testScenePath = "Assets/Scenes/Test Scene.unity";

    /// <summary>
    /// Load Test Scene on Click
    /// </summary>
    public void clickAction()
    {
        SceneManager.LoadScene(testScenePath);
    }
}
