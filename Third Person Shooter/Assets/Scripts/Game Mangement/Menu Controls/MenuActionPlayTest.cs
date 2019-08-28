using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionPlayTest : MonoBehaviour, IMenuAction
{
    // Constant Scene Paths
    private const string testScenePath = "Assets/Scenes/Test Scene.unity";

    // Load Test Scene on Click
    public void clickAction()
    {
        SceneManager.LoadScene(testScenePath);
    }
}
