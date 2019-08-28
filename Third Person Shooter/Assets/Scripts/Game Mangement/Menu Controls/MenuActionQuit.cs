using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActionQuit : MonoBehaviour, IMenuAction
{
    // Quit game on click
    public void clickAction()
    {
        Application.Quit();
    }
}
