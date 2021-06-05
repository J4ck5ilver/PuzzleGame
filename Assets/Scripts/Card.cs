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

    private bool holdingCardTimerActive = false;

    private void Awake()
    {
        beginSnapOffset = transform.GetComponent<RectTransform>().rect.height;
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

        if (direction != Direction.None)
        {
            RectTransform directionTransform = transform.Find("directionSprite").GetComponent<RectTransform>();
            Vector3 rotation = new Vector3(0, 0, UtilsClass.DirectionToDegrees(direction));
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
        else
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
