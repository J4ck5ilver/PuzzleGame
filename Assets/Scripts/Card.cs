using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    [SerializeField]private int numberOfMoves;
    [SerializeField] private CardType cardType;
    [SerializeField] private Direction direction;


    public event EventHandler<PanelEventArgs> CardDragEvent;
    public event EventHandler<PanelEventArgs> CardClickedEvent;

    private bool lockCardClick = false;

    public void SetData(CardDescriptor descriptor)
    {
        numberOfMoves = descriptor.numberOfMoves;
        cardType = descriptor.type;
        direction = descriptor.direction;
        UpdateVisuals();
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
        cardData.type          = cardType;
        cardData.direction     = direction;

        return cardData;
    }

    public void SetTheme(CardThemeSO theme)
    {
        if (theme != null)
        {

        }
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(!lockCardClick)
        {
            PanelEventArgs data = new PanelEventArgs();
            data.pointerData = pointerEventData;
            data.senderTransform = transform;
            CardClickedEvent?.Invoke(this, data);

        }
    }


    public void OnDrag(PointerEventData pointerEventData)
    {
        PanelEventArgs data = new PanelEventArgs();
        data.pointerData = pointerEventData;
        CardDragEvent?.Invoke(this, data);
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        lockCardClick = true;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        lockCardClick = false;
    }
}
