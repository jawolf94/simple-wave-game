using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    /*
     * Desc: Clickable Text is a class which defines behavior for text which the player can click.
     * Behavior is defined for Click & Hover.
     * Class must be used inconjuction with IMenuAction Compoenent
     */

    //Public Unity set Vars

    /// <summary>
    /// The color to set when the pointer is over the text.
    /// </summary>
    public Color colorOnHover;

    /// <summary>
    /// Font size increase on pointer hover.
    /// </summary>
    public int sizeIncreaseOnHover;

    //Private instance variables

    /// <summary>
    /// Text Component attached to the GameObject.
    /// </summary>
    private Text text;

    /// <summary>
    /// IMenuAction attached to GameObject 
    /// </summary>
    private IMenuAction menuAction;

    /// <summary>
    /// The starting font color of the Text component
    /// </summary>
    private Color startColor;

    /// <summary>
    /// The starting font size of the Text component
    /// </summary>
    private int startFontSize;


    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        menuAction = GetComponent<IMenuAction>();

        startColor = text.color;
        startFontSize = text.fontSize;
    }

    // Function is called when pointer enters the text object area
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Set color and font size
        text.color = colorOnHover;
        text.fontSize += sizeIncreaseOnHover;
    }

    // Function is called when pointer exits the text area
    public void OnPointerExit(PointerEventData eventData)
    {
        //Revert color and font size to starting values 
        text.color = startColor;
        text.fontSize = startFontSize;
    }

    //Execute action on click
    public void OnPointerClick(PointerEventData eventData)
    {
        //Click actions are defined by clickAction() funtion in attacted IMenuAction Component.
        //Actions are user defined.
        menuAction.clickAction();
    }
}
