using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CardSlot : MonoBehaviour, IPointerEnterHandler
{


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

    void OnMouseOver()
    {
        Debug.Log("Yo1");
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");
    }

    private void OnMouseDrag()
    {
        Debug.Log("Yo2");
    }

    private void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    Debug.Log("Yo");
        //}
        //else if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log("Yo2");

        //}
    }

    void Awake()
    {
       // background = transform.Find("background").GetComponent<Image>();
    }


}
