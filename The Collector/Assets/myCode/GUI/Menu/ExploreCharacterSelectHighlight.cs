using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ExploreCharacterSelectHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image highlight;
    [SerializeField] private bool isHighlighted;
    void Start()
    {
        //highlight = GameObject.Find("ScreenHighlight").GetComponent<Image>();
        highlight.enabled = false;
        isHighlighted = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //isHighlighted = true;
        //highlight.enabled = isHighlighted;
        highlight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isHighlighted) highlight.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("highlight toggled");
        isHighlighted = !isHighlighted;
        //highlight.enabled = isHighlighted;
        highlight.enabled = true;
    }
    public void ToggleFromOutside()
    {
        isHighlighted = !isHighlighted;
        //highlight.enabled = isHighlighted;
        highlight.enabled = isHighlighted;
    }
    public void ResetHighlight()
    {
        isHighlighted=false;
        highlight.enabled = isHighlighted;
    }
}
