using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class FollowMouse : MonoBehaviour
{
    public event EventHandler<PanelEventArgs> HoldObjectiveOnLeftSideOfScreen;
    public event EventHandler<PanelEventArgs> HoldObjectiveOnRightSideOfScreen;

    public event EventHandler<EventArgs> OnDestroyed;
    private RectTransform canvasRectTransform;

    //From the center of the screen
    private const float scrollRightActivatePercentage = 0.55f;
    private const float scrollLeftActivatePercentage = 0.45f;

    private float yHoldOffset = 0.0f;

    private void Awake()
    {
        canvasRectTransform = UtilsClass.GetTopmostCanvas(this).GetComponent<RectTransform>();
        yHoldOffset = transform.GetComponent<RectTransform>().rect.height * 1.5f;


        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(gameObject.GetComponent<RectTransform>().rect.size.x * 3.4f, gameObject.GetComponent<RectTransform>().rect.size.y * 100.5f);
    }

    void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
        transform.position = new Vector3(transform.position.x, transform.position.y + yHoldOffset, 0.0f);

        if(transform.localPosition.x < (canvasRectTransform.rect.xMin * (1.0f - scrollLeftActivatePercentage)))
        {

            float subOffset = (canvasRectTransform.rect.xMin * (1.0f - scrollLeftActivatePercentage));
            float posRange = Math.Abs(canvasRectTransform.rect.xMin - subOffset);
            float posCalc = Mathf.Abs(transform.localPosition.x - subOffset);
            float interpolateVal = Mathf.Clamp(posCalc / posRange,0.0f,1.0f);
    
            PanelEventArgs arg = new PanelEventArgs();
            arg.floatData = interpolateVal;

            HoldObjectiveOnLeftSideOfScreen?.Invoke(this, arg);
        }
        else if (transform.localPosition.x > (canvasRectTransform.rect.xMax * (1.0f - scrollRightActivatePercentage)))
        {

            float subOffset = (canvasRectTransform.rect.xMax * (1.0f - scrollRightActivatePercentage));
            float posRange = Math.Abs(canvasRectTransform.rect.xMax - subOffset);
            float posCalc = Mathf.Abs(transform.localPosition.x - subOffset);
            float interpolateVal = Mathf.Clamp(posCalc / posRange, 0.0f, 1.0f);

            PanelEventArgs arg = new PanelEventArgs();
            arg.floatData = interpolateVal;

            HoldObjectiveOnRightSideOfScreen?.Invoke(this, arg);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
