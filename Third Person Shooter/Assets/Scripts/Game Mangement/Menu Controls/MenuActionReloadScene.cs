using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionReloadScene : MonoBehaviour, IMenuAction
{
    public void clickAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().path);
    }
}
