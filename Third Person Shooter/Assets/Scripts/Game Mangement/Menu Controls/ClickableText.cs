using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Color colorOnHover;
    public int sizeIncreaseOnHover;


    private Text text;
    private Color startColor;
    private int startFontSize;
    private IMenuAction menuAction;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        menuAction = GetComponent<IMenuAction>();

        startColor = text.color;
        startFontSize = text.fontSize;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = colorOnHover;
        text.fontSize += sizeIncreaseOnHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = startColor;
        text.fontSize = startFontSize;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        menuAction.clickAction();
    }
}
