using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private int numberOfMoves;
    [SerializeField] private CardType cardType;
    [SerializeField] private Direction direction;


    public event EventHandler<PanelEventArgs> CardDragEvent;
    public event EventHandler<PanelEventArgs> CardClickedEvent;
    public event EventHandler<PanelEventArgs> CardHoldCardEvent;

    private Vector3 beginDragPosition = new Vector3();

    private bool clickLocked = false;

    private float beginSnapOffset;

    private float holdCardDelay = 0.35f;
    private float holdCardTimer = 0.0f;

    private float rotationOffset = 0.0f;

    private Direction rotationDirection;

    private bool holdingCardTimerActive = false;

    private void Awake()
    {
        beginSnapOffset = transform.GetComponent<RectTransform>().rect.height * 1.5f;
    }
    private void Update()
    {
        if (holdingCardTimerActive)
        {
            holdCardTimer += Time.deltaTime;
            if (holdCardTimer >= holdCardDelay)
            {
                SendCardHoldEvent();
            }
        }

    }

    public void SetAlfa(float alfa)
    {
        // Can be optimized

        foreach(Transform child in transform)
        {

            Image tmpImage;
            TextMeshProUGUI tmpText;
            if (child.childCount != 0)
            {
                foreach (Transform subChild in child)
                {
                    tmpImage = subChild.GetComponent<Image>();
                    tmpText = subChild.GetComponent<TextMeshProUGUI>();
                    if (tmpImage != null)
                    {
                        Color tmpColor = tmpImage.color;
                        tmpColor.a = alfa;
                        tmpImage.color = tmpColor;
                    }
                    if (tmpText != null)
                    {
                        Color tmpColor = tmpText.color;
                        tmpColor.a = alfa;
                        tmpText.color = tmpColor;
                    }
                }
            }



            tmpImage = child.GetComponent<Image>();
            tmpText =   child.GetComponent<TextMeshProUGUI>();
            if (tmpImage != null)
            {
                Color tmpColor = tmpImage.color;
                tmpColor.a = alfa;
                tmpImage.color = tmpColor;
            }
            if(tmpText != null)
            {
                Color tmpColor = tmpText.color;
                tmpColor.a = alfa;
                tmpText.color = tmpColor;
            }
        }
    }


    private void SendCardHoldEvent()
    {
        DeactivateHoldCardTimer();
        PanelEventArgs tmpArgs = new PanelEventArgs();
        tmpArgs.senderTransform = this.transform;
        CardHoldCardEvent?.Invoke(this, tmpArgs);
    }

    private void DeactivateHoldCardTimer()
    {
        holdingCardTimerActive = false;
        holdCardTimer = 0.0f;
    }

    public void SetData(CardDescriptor descriptor)
    {
        numberOfMoves = descriptor.numberOfMoves;
        cardType = descriptor.type;
        direction = descriptor.direction;
        rotationDirection = direction;
        UpdateVisuals();
        CardThemeSO theme = AssetManager.GetCardTheme(ThemeManager.Instance.GetCurrentCardTheme());
        SetTheme(theme);
    }



    public void SetCardHoldDelay(float delayTime)
    {
        holdCardDelay = delayTime;
    }

    private void UpdateVisuals()
    {


        TextMeshProUGUI numberOfMovesText = transform.Find("numberOfMovesSprite").Find("text").GetComponent<TextMeshProUGUI>();
        numberOfMovesText.text = Mathf.Clamp(numberOfMoves, GameConstants.minNumberOfMoves, GameConstants.maxNumberOfMoves).ToString();

        if (rotationDirection != Direction.None)
        {
            RectTransform directionTransform = transform.Find("directionSprite").GetComponent<RectTransform>();
            Vector3 rotation = new Vector3(0, 0, (UtilsClass.DirectionToDegrees(rotationDirection) + rotationOffset));
            directionTransform.rotation = Quaternion.Euler(rotation);
        }

    }
    public CardDescriptor GetData()
    {
        CardDescriptor cardData = new CardDescriptor();

        cardData.numberOfMoves = numberOfMoves;
        cardData.type = cardType;
        cardData.direction = direction;

        return cardData;
    }

    public void SetDirectionArrowRotationOffset(float offset)
    {
        rotationOffset = offset;
        //int newDir = (int)latitude - (int)direction;

        //if(Math.Abs(newDir) == 2)
        //{
        //    //Debug.Log("180");
        //    rotationOffset = 180.0f;
        //}
        //else if (Math.Abs(newDir) == 3)
        //{
        //    rotationOffset = -90.0f;
        //    //Debug.Log("-90");
        //}
        //else if (Math.Abs(newDir) == 1)
        //{
        //    rotationOffset = 90.0f;
        //    //Debug.Log("90");
        //}
        //else
        //{
        //    rotationOffset = 0.0f;
        //}

          //  rotationDirection = GetNewFromDirections(direction, latitude);
        UpdateVisuals();
    }

    private Direction GetNewFromDirections(Direction from, Direction To)
    {

        



        return Direction.East;
    }

    public void SetTheme(CardThemeSO theme)
    {
        if (theme != null)
        {
            transform.Find("background").GetComponent<Image>().sprite = theme.backgroundSprite;
        }
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!clickLocked)
        {
            PanelEventArgs data = new PanelEventArgs();
            data.pointerData = pointerEventData;
            data.senderTransform = transform;
            CardClickedEvent?.Invoke(this, data);
        }
    }


    public void OnDrag(PointerEventData pointerEventData)
    {

        if (beginSnapOffset < (pointerEventData.position.y - transform.position.y))
        {
            clickLocked = false;
            SendCardHoldEvent();
        }
        else if((Mathf.Abs(pointerEventData.delta.x / 4.0f)) > pointerEventData.delta.y)
        {
            PanelEventArgs data = new PanelEventArgs();
            data.pointerData = pointerEventData;
            CardDragEvent?.Invoke(this, data);
        }
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (holdingCardTimerActive)
        {
            DeactivateHoldCardTimer();
        }
        beginDragPosition = pointerEventData.position;

        clickLocked = true;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        beginDragPosition = new Vector3();
        clickLocked = false;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (holdingCardTimerActive)
        {
            DeactivateHoldCardTimer();
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        holdingCardTimerActive = true;
    }
}
