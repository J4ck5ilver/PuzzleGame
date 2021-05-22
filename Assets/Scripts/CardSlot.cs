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
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void OnDrag(PointerEventData eventData)
    {
        PanelEventArgs pointerData = new PanelEventArgs();
        pointerData.pointerData = eventData;
        OnCardSlotDrag?.Invoke(this, pointerData);
    }




}
