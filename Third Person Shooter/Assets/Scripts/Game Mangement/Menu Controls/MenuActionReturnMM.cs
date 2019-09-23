﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActionReturnMM : MonoBehaviour, IMenuAction
{
    private static string mainMenuPath = "Assets/Scenes/Main Menu.unity";


    // Loads main menu when clicked
    public void clickAction()
    {
        SceneManager.LoadScene(mainMenuPath);
    }
}