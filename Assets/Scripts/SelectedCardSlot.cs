using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectedCardSlot : MonoBehaviour
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
        Vector3 offset = new Vector3(collision.GetComponent<RectTransform>().rect.width / 2.0f, 0.0f, 0.0f);
        arg.poitionData2D = (transform.position - collision.transform.position + offset);
        return arg;
    }


}
