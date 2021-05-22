using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDragHandler
{

    public event EventHandler<PanelEventArgs> OnCardSlotDrag;

    private bool isSelected = false;

    public Transform GetCard()
    {
        Transform returnValue = null;
        foreach(Transform child in transform)
        {
            Card hasComponent = child.GetComponent<Card>();
            if(hasComponent != null)
            {
                returnValue = child;
                break;
            }
        }
        return returnValue;
    }

    public void SetIsSelected(bool selected)
    {
        isSelected = selected;
        UpdateVisuals();
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    private void UpdateVisuals()
    {
        Image backgroundImage = transform.Find("background").GetComponent<Image>();
        if (isSelected)
        {
            backgroundImage.color = Color.green;
        }
        else
        {
            backgroundImage.color = Color.red;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        PanelEventArgs pointerData = new PanelEventArgs();
        pointerData.pointerData = eventData;
        OnCardSlotDrag?.Invoke(this, pointerData);
    }


    public void SetTheme(CardSlotThemeSO theme)
    {
        if (theme != null)
        {
            transform.Find("background").GetComponent<Image>().sprite = theme.backgroundSprite;
            UpdateVisuals();
        }
    }




}
