using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionPlayGame : MonoBehaviour, IMenuAction
{
    // Constant Scene Paths
    private const string testScenePath = "Assets/Scenes/Level 1.unity";

    // Load Test Scene on Click
    public void clickAction()
    {
        SceneManager.LoadScene(testScenePath);
    }
}
