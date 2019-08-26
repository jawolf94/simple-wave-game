using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActionQuit : MonoBehaviour, IMenuAction
{
    public void clickAction()
    {
        Application.Quit();
    }
}
