using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayButton : MonoBehaviour, IPointerClickHandler
{

    public event EventHandler<EventArgs> OnClick;

    bool test = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(this, EventArgs.Empty);

        if (test)
        {
            transform.Find("background").GetComponent<Image>().color = Color.green;
        }
        else
        {
            transform.Find("background").GetComponent<Image>().color = Color.red;
        }
        test = !test;
    }
}
