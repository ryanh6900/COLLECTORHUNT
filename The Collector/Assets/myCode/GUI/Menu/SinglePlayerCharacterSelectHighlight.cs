using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CharacterSelectHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image highlight;
    void Start()
    {
        highlight = GetComponent<Image>();
        highlight.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.enabled = false;
    }
}
