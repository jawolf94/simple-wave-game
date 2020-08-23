using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionPlayGame : MonoBehaviour, IMenuAction
{
    /// <summary>
    /// Constant Scene Path
    /// </summary>    
    private const string testScenePath = "Assets/Scenes/Level 1.unity";

    /// <summary>
    /// Load Test Scene on Click
    /// </summary>
    public void clickAction()
    {
        SceneManager.LoadScene(testScenePath);
    }
}
