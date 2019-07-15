using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction
{
    /// <summary>
    /// A reference to the controller associated with the Player Component
    /// </summary>
    PlayerController PlayerController { get; set; }

    /// <summary>
    /// Executes before the PlayerController checks for Player Input.
    /// All actions that should be done before the player performs an action should be placed here. 
    /// </summary>
    void PreAction();

    /// <summary>
    /// Executes After the PlayerController checks for Player Input
    /// All actions that should be done after the player performs an action should be placed here.
    /// </summary>
    void PostAction();
}
