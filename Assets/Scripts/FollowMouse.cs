using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class FollowMouse : MonoBehaviour
{
    public event EventHandler<EventArgs> HoldObjectiveOnLeftSideOfScreen;
    public event EventHandler<EventArgs> HoldObjectiveOnRightSideOfScreen;

    public event EventHandler<EventArgs> OnDestroyed;
    private RectTransform canvasRectTransform;

    private const float scrollRightActivatePercentage = 0.5f;
    private const float scrollLeftActivatePercentage = 0.4f;

    private void Awake()
    {
        canvasRectTransform = UtilsClass.GetTopmostCanvas(this).GetComponent<RectTransform>();
    }
    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this, EventArgs.Empty);
    }
    void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        // Debug.Log(UtilsClass.GetMouseWorldPosition());
        // Debug.Log("Local Pos " + transform.localPosition);
        //  mainCanvas.GetComponent<RectTransform>().rect.center

        Debug.Log("%" + (canvasRectTransform.rect.xMin * (1.0f - scrollLeftActivatePercentage)));
        if(transform.localPosition.x < (canvasRectTransform.rect.xMin * (1.0f - scrollLeftActivatePercentage)))
        {
            HoldObjectiveOnLeftSideOfScreen?.Invoke(this, EventArgs.Empty);
        }
        else if (transform.localPosition.x > (canvasRectTransform.rect.xMax * (1.0f - scrollRightActivatePercentage)))
        {
            HoldObjectiveOnRightSideOfScreen?.Invoke(this,EventArgs.Empty);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            Destroy(this.gameObject);
        }


        

    }
}
