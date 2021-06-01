using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectedCardSlot : MonoBehaviour
{



    public event EventHandler<PanelEventArgs> CurrentCollision2DPoitionEvent;
    private void OnTriggerStay2D(Collider2D collision)
    {

        
        PanelEventArgs arg = new PanelEventArgs();
        Vector3 offset = new Vector3(collision.GetComponent<RectTransform>().rect.width / 2.0f,0.0f,0.0f);
        arg.poitionData2D = (transform.position - collision.transform.position + offset);
        CurrentCollision2DPoitionEvent?.Invoke(this, arg);


    }



}
