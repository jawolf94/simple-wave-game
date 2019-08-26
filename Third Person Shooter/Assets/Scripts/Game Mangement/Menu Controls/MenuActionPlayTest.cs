using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionPlayTest : MonoBehaviour, IMenuAction
{
    private const string testScenePath = "Assets/Scenes/Test Scene.unity";

    public void clickAction()
    {
        SceneManager.LoadScene(testScenePath);
    }
}
