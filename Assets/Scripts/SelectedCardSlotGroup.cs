using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class SelectedCardSlotGroup : MonoBehaviour
{



    public event EventHandler<PanelEventArgs> CollisionEnterEvent;
    public event EventHandler<EventArgs> CollisionExitEvent;
    public event EventHandler<PanelEventArgs> CollisionStayEvent;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FollowMouse>() != null)
        {
            CollisionEnterEvent?.Invoke(this, GetPanelPositionArgs(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<FollowMouse>() != null)
        {
            if(collision.gameObject.GetComponent<BoxCollider2D>().enabled)
            {
                CollisionExitEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<FollowMouse>() != null)
        {
            CollisionStayEvent?.Invoke(this, GetPanelPositionArgs(collision));
        }
    }


    private PanelEventArgs GetPanelPositionArgs(Collider2D collision)
    {
        PanelEventArgs arg = new PanelEventArgs();

        int returnIndex = 0;
        float closestCardSlot = 10000.0f;
        int intLargetsIndex = 0;

        foreach (Transform cardSlot in transform)
        {
            CardSlot cardSlotComponent = cardSlot.GetComponent<CardSlot>();
            if (cardSlot.gameObject.activeInHierarchy && cardSlotComponent != null)
            {


                int currentSiblingIndex = cardSlot.GetSiblingIndex();

                if (intLargetsIndex <= currentSiblingIndex)
                {
                    intLargetsIndex = currentSiblingIndex;
                }

                float distance = Mathf.Abs(cardSlot.position.x - collision.transform.position.x);
                if(distance < closestCardSlot)
                {
                    returnIndex = currentSiblingIndex;
                    closestCardSlot = distance;
                }

            }
        }




        if (returnIndex == intLargetsIndex && returnIndex != 0)
        {
            returnIndex++;
        }



            arg.intData = returnIndex;


        return arg;
    }

}
