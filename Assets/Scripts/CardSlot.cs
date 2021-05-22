using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDragHandler
{

    public event EventHandler<PointerEventArgs> OnCardSlotDrag;

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


    public void OnDrag(PointerEventData eventData)
    {
        PointerEventArgs pointerData = new PointerEventArgs();
        pointerData.pointerData = eventData;
        OnCardSlotDrag?.Invoke(this, pointerData);
    }




}
