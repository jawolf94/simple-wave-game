using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionReloadScene : MonoBehaviour, IMenuAction
{
    // Reloads active scene when clicked
    public void clickAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().path);
    }
}
