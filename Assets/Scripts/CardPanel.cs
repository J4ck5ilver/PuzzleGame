using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardPanel : MonoBehaviour, IDragHandler
{

    [SerializeField] private Transform cardSlotPreFab;

    private const int paddingOffset = 5;

    private GridLayoutGroup cardSlotsGridLayoutGroup;
    private float cardSlotWidth;

    private Transform selectionCardSlots;

    private Transform currentCardInPlay;

    private Transform outputCardSlot;
    private Transform selectedCardSlots;
    private Transform nonSelectedCardSlots;
    private Transform movingCardSeparator;

    private Transform buttonTransform;

    private CardSortOrder sortOrder;

    private CardPanelState currentState;

    private bool scrollingEnabled = true;
    private bool carSelectionEnabled = true;



    private void Awake()
    {
        sortOrder = CardSortOrder.CardType;
        currentState = CardPanelState.Select;
        InitCardSlotData();

        buttonTransform = transform.Find("playButton");
        PlayButton component = buttonTransform.GetComponent<PlayButton>();
        component.OnClick += OnPlayButtonClicked;


    }

    private void Start()
    {

        RemoveAllCardSlots();
        InitNewCards();
        UpdateSlotsOffset();
        SortNonSelectedCards();
        UpdateMovingCardSeparator();

        CardPanelThemeSO theme = AssetManager.GetCardPanelTheme(ThemeManager.Instance.GetCurrentPanelTheme());
        SetTheme(theme);

        //UpdateUIFunction() // from themeManager
    }

    public void SetTheme(CardPanelThemeSO theme)
    {
        if (theme != null)
        {
            transform.Find("background").GetComponent<Image>().sprite = theme.panelBackgroundSprite;
            SetThemeForCardSlots(theme.cardSlotTheme);
        }
    }

    #region InteractionLogic

    public void OnCardClicked(object sender, PanelEventArgs args)
    {
        if (carSelectionEnabled)
        {

            CardSlot cardSlot = args.senderTransform.parent.GetComponent<CardSlot>();
            if (cardSlot.IsSelected())
            {
                cardSlot.SetIsSelected(false);
                cardSlot.transform.SetParent(nonSelectedCardSlots.transform);
                SortNonSelectedCards();
            }
            else
            {
                cardSlot.SetIsSelected(true);
                cardSlot.transform.SetParent(selectedCardSlots.transform);
            }
            UpdateSlotsOffset();
            UpdateMovingCardSeparator();
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveScrollViewHorizontaly(eventData.delta.x);
    }
    public void OnCardDrag(object sender, PanelEventArgs args)
    {
        MoveScrollViewHorizontaly(args.pointerData.delta.x);
    }

    public void OnCardSlotDrag(object sender, PanelEventArgs args)
    {
        MoveScrollViewHorizontaly(args.pointerData.delta.x);
    }

    private void OnPlayButtonClicked(object sender, EventArgs e)
    {
        if (HasSelectedCards())
        {
            SetState(CardPanelState.Play);
        }

    }

    private bool HasSelectedCards()
    {
        bool returnvalue = false;
        foreach (Transform cardSlot in selectedCardSlots)
        {
            if (cardSlot.GetComponent<CardSlot>() != null && cardSlot.gameObject.activeInHierarchy)
            {
                returnvalue = true;
                break;
            }
        }
        return returnvalue;
    }




    #endregion


    private void MoveScrollViewHorizontaly(float horizontalVal)
    {
        if (scrollingEnabled)
        {
            RectTransform cardSlotTransform = selectionCardSlots.GetComponent<RectTransform>();
            Vector3 newPosition = new Vector3(horizontalVal, 0.0f, 0.0f);
            cardSlotTransform.position = cardSlotTransform.position + newPosition;
        }
    }

    private void EnableScrolling(bool state)
    {
        scrollingEnabled = state;
        transform.GetComponent<ScrollRect>().enabled = state;
    }
    private void EnableCardSelection(bool state)
    {
        carSelectionEnabled = state;
    }
    private void EnablePlayButton(bool state)
    {
        buttonTransform.GetComponent<PlayButton>().enabled = state;
        buttonTransform.gameObject.SetActive(state);
    }



    #region CardSlotLogic
    private void InitCardSlotData()
    {

        Transform cardSlotView = transform.Find("cardSlotViewport");

        selectionCardSlots = cardSlotView.Find("selectionCardSlots");
        movingCardSeparator = cardSlotView.Find("movingCardSeparator");
        outputCardSlot = cardSlotView.Find("outputSlot");

        selectedCardSlots = selectionCardSlots.Find("selected");
        nonSelectedCardSlots = selectionCardSlots.Find("nonSelected");



        cardSlotsGridLayoutGroup = selectionCardSlots.GetComponent<GridLayoutGroup>();

        cardSlotWidth = cardSlotPreFab.GetComponent<RectTransform>().rect.width;

        cardSlotsGridLayoutGroup.padding.left = Mathf.CeilToInt(cardSlotWidth / 2.0f) + paddingOffset * 2;
        cardSlotsGridLayoutGroup.padding.right = cardSlotsGridLayoutGroup.padding.left + Mathf.CeilToInt(outputCardSlot.GetComponent<RectTransform>().rect.width);

        selectedCardSlots.GetComponent<GridLayoutGroup>().spacing = new Vector2(cardSlotWidth + paddingOffset, 0.0f);
        nonSelectedCardSlots.GetComponent<GridLayoutGroup>().spacing = new Vector2(cardSlotWidth + paddingOffset, 0.0f);
    }


    public void NextCard()
    {
        bool noMoreCards = true;

        foreach (Transform cardSlot in selectedCardSlots)
        {
            noMoreCards = true;
            if (cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>() != null)
            {

                if(currentCardInPlay != null)
                {
                    DestroyImmediate(currentCardInPlay.gameObject);
                }

                currentCardInPlay = Instantiate(cardSlot.GetComponent<CardSlot>().GetCard(), outputCardSlot);
                cardSlot.gameObject.SetActive(false);

                RectTransform rectTransCard = currentCardInPlay.GetComponent<RectTransform>();
                rectTransCard.position = new Vector3(0, 0, 0);
                rectTransCard.anchoredPosition = new Vector2(0, 0);
                rectTransCard.anchoredPosition3D = new Vector3(0, 0, 0);

                noMoreCards = false;
                break;
            }
        }

        if(noMoreCards && currentCardInPlay != null)
        {
            DestroyImmediate(currentCardInPlay.gameObject);
            currentCardInPlay = null;
        }

        UpdateSlotsOffset();
        UpdateMovingCardSeparator();
    }


    public void SetState(CardPanelState state)
    {
        switch (state)
        {
            case CardPanelState.Play:
                currentState = state;

                //Vector3 oldPosition = selectionCardSlots.GetComponent<RectTransform>().position;
                EnableScrolling(false);
                EnablePlayButton(false);
                EnableCardSelection(false);
                NextCard();
                //selectionCardSlots.GetComponent<RectTransform>().position = new Vector3(0.0f, oldPosition.y, oldPosition.z);
                // UpdateSlotsOffset();

                Vector3 oldPosition = selectionCardSlots.GetComponent<RectTransform>().position;
                RectTransform rectTransform = selectionCardSlots.GetComponent<RectTransform>();
                rectTransform.position = new Vector3(0.0f, oldPosition.y, oldPosition.z);
                rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
                rectTransform.anchoredPosition3D  = new Vector3(0.0f, 0.0f, 0.0f);
                break;

            case CardPanelState.Reset:

                EnableScrolling(true);
                EnablePlayButton(true);
                EnableCardSelection(true);
                ActivateAllDisabledSlotsInSelectedGroup();
                UpdateSlotsOffset();
                SortNonSelectedCards();

                break;

            case CardPanelState.Select:


                break;
            case CardPanelState.NextStep:
                EnableScrolling(true);
                EnablePlayButton(true);
                EnableCardSelection(true);
                MoveAllDisabledSlotsInSelectedGroupToNonSelected();
                UpdateSlotsOffset();
                SortNonSelectedCards();
                break;
        }
    }

    private void MoveAllDisabledSlotsInSelectedGroupToNonSelected()
    {
        List<Transform> slotsToMove = new List<Transform>();

        foreach (Transform cardSlot in selectedCardSlots)
        {
            if (!cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>() != null)
            {
                slotsToMove.Add(cardSlot);
            }
        }

        foreach (Transform cardSlot in slotsToMove)
        {
            cardSlot.GetComponent<CardSlot>().SetIsSelected(false);
            cardSlot.transform.SetParent(nonSelectedCardSlots.transform);
        }
        SortNonSelectedCards();

    }

    private void ActivateAllDisabledSlotsInSelectedGroup()
    {
        foreach (Transform cardSlot in selectedCardSlots)
        {
            if (!cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>() != null)
            {
                cardSlot.gameObject.SetActive(true);
            }
        }
    }

    private void InitNewCards()
    {
        List<Transform> cards = CardManager.Instance.GetStartCards();

        foreach (Transform card in cards)
        {
            Transform newCardSlot = Instantiate(cardSlotPreFab, nonSelectedCardSlots.transform);

            RectTransform rectTransCardSlot = newCardSlot.GetComponent<RectTransform>();
            rectTransCardSlot.position = new Vector3(0, 0, 0);
            rectTransCardSlot.anchoredPosition = new Vector2(0, 0);

            CardSlot cardSlotComponent = newCardSlot.GetComponent<CardSlot>();
            cardSlotComponent.OnCardSlotDrag += OnCardSlotDrag;
            cardSlotComponent.SetIsSelected(false);

            Transform newCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity, newCardSlot.transform);
            CardManager.Instance.AddToCardsInPlay(newCard);

            Card cardComponent = newCard.GetComponent<Card>();
            cardComponent.SetData(card.GetComponent<Card>().GetData());
            cardComponent.GetComponent<Card>().CardDragEvent += OnCardDrag;
            cardComponent.GetComponent<Card>().CardClickedEvent += OnCardClicked;

            RectTransform rectTransCard = newCard.GetComponent<RectTransform>();
            rectTransCard.position = new Vector3(0, 0, 0);
            rectTransCard.anchoredPosition = new Vector2(0, 0);
            //Update Card UI from ThemeManager
        }
    }

    private void RemoveAllCardSlots()
    {
        foreach (Transform cardSlot in selectedCardSlots)
        {
            CardSlot hasCardSlot = cardSlot.GetComponent<CardSlot>();
            if (hasCardSlot != null)
            {
                Destroy(cardSlot);
            }
        }
        foreach (Transform cardSlot in nonSelectedCardSlots)
        {
            CardSlot hasCardSlot = cardSlot.GetComponent<CardSlot>();
            if (hasCardSlot != null)
            {
                Destroy(cardSlot);
            }
        }
    }

    private void UpdateSlotsOffset()
    {

        int nrOfSlotsInSelected = GetNumberOfActiveCardSlotsInSelected();
        int nrOfSlotsInNonSelected = GetNumberOfActiveCardSlotsInNonSelected();

        float nonSelectedPanelOffset = ((nrOfSlotsInSelected) * (Mathf.CeilToInt(cardSlotWidth) + paddingOffset)) + (paddingOffset * 2);
        cardSlotsGridLayoutGroup.spacing = new Vector2(nonSelectedPanelOffset, 0.0f);

        int viewOffset = nrOfSlotsInNonSelected * Mathf.CeilToInt((cardSlotWidth + paddingOffset)) - paddingOffset * 6; //(left, right + middle seperator removal)
        cardSlotsGridLayoutGroup.padding.left = viewOffset;

    }

    private void UpdateMovingCardSeparator()
    {
        Transform cardSlottransform = null;

        foreach (Transform cardSlot in selectedCardSlots)
        {
            if (cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>() != null)
            {
                cardSlottransform = cardSlot;
            }
        }

        if (cardSlottransform == null)
        {
            foreach (Transform cardSlot in nonSelectedCardSlots)
            {
                if (cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>() != null)
                {
                    cardSlottransform = cardSlot;
                    break;
                }
            }
        }

        if (cardSlottransform != null)
        {
            movingCardSeparator.SetParent(cardSlottransform.transform);
            movingCardSeparator.position = new Vector3(0.0f, 0.0f, 0.0f);

            RectTransform separatorRectTransform = movingCardSeparator.GetComponent<RectTransform>();

            float xOffset = (cardSlottransform.GetComponent<CardSlot>().GetWidth() / 2.0f) + ((float)paddingOffset * 1.5f);

            if (cardSlottransform.GetComponent<CardSlot>().IsSelected())
            {
                xOffset *= -1;
            }

            separatorRectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
            separatorRectTransform.anchoredPosition3D = new Vector3(0.0f, 0.0f, 0.0f);
            separatorRectTransform.localPosition = new Vector3(xOffset, 0.0f, 0.0f);
        }


    }

    public void SetSortOrder(CardSortOrder sortOrder)
    {
        this.sortOrder = sortOrder;
    }

    public void SortNonSelectedCards()
    {
        // This could probably be optimized.
        switch (sortOrder)
        {
            case CardSortOrder.CardType:

                Dictionary<CardType, Dictionary<Direction, Dictionary<int, List<Transform>>>> sortDictionary = new Dictionary<CardType, Dictionary<Direction, Dictionary<int, List<Transform>>>>();
                var cardTypes = CardType.GetValues(typeof(CardType));
                var directionTypes = Direction.GetValues(typeof(Direction));

                foreach (CardType type in cardTypes)
                {
                    sortDictionary.Add(type, new Dictionary<Direction, Dictionary<int, List<Transform>>>());
                    foreach (Direction direction in directionTypes)
                    {
                        sortDictionary[type][direction] = new Dictionary<int, List<Transform>>();
                        for (int i = GameConstants.minNumberOfMoves; i <= GameConstants.maxNumberOfMoves; i++)
                            sortDictionary[type][direction][i] = new List<Transform>();
                    }
                }

                foreach (Transform cardSlot in nonSelectedCardSlots)
                {
                    CardSlot hasComponent = cardSlot.GetComponent<CardSlot>();
                    if (hasComponent != null)
                    {
                        Transform tmpCard = hasComponent.GetCard();
                        if (tmpCard != null)
                        {
                            CardDescriptor cardDesc = tmpCard.GetComponent<Card>().GetData();
                            sortDictionary[cardDesc.type][cardDesc.direction][cardDesc.numberOfMoves].Add(cardSlot);
                        }
                        else
                        {
                            Destroy(cardSlot.gameObject);
                            Debug.LogError("Sorting CardSlot with no card");
                        }
                    }
                }


                foreach (CardType type in cardTypes)
                {
                    foreach (Direction direction in directionTypes)
                    {
                        for (int i = GameConstants.minNumberOfMoves; i <= GameConstants.maxNumberOfMoves; i++)
                        {
                            foreach (Transform cardSlot in sortDictionary[type][direction][i])
                            {
                                cardSlot.SetSiblingIndex(0);
                            }
                        }
                    }
                }

                break;
            case CardSortOrder.Direction:

                break;
            case CardSortOrder.NumberOfMoves:

                break;
        }
    }

    private int GetNumberOfActiveCardSlots()
    {
        return GetNumberOfActiveCardSlotsInSelected() + GetNumberOfActiveCardSlotsInNonSelected();
    }

    private int GetNumberOfActiveCardSlotsInSelected()
    {
        int returnValue = 0;
        foreach (Transform cardSlot in selectedCardSlots)
        {
            if (cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>() != null)
            {
                returnValue++;
            }
        }
        return returnValue;
    }

    private int GetNumberOfActiveCardSlotsInNonSelected()
    {
        int returnValue = 0;
        foreach (Transform cardSlot in nonSelectedCardSlots)
        {
            if (cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>() != null)
            {
                returnValue++;
            }
        }
        return returnValue;
    }


    private Transform GetCarsSlotInNonSelectedWithCard(Transform cardToFind)
    {
        Transform returnValue = null;

        foreach (Transform cardSlot in nonSelectedCardSlots)
        {
            if (cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>().GetCard().transform == cardToFind)
            {
                returnValue = cardSlot;
                break;
            }
        }
        return returnValue;
    }

    private Transform GetCarsSlotInSelectedWithCard(Transform cardToFind)
    {
        Transform returnValue = null;

        foreach (Transform cardSlot in selectedCardSlots)
        {
            if (cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>().GetCard().transform == cardToFind)
            {
                returnValue = cardSlot;
                break;
            }
        }
        return returnValue;
    }

    private Transform SetThemeForCardSlots(CardSlotThemeSO theme)
    {
        Transform returnValue = null;

        foreach (Transform cardSlot in nonSelectedCardSlots)
        {
            CardSlot hasComponent = cardSlot.GetComponent<CardSlot>();
            if (cardSlot.gameObject.activeInHierarchy && hasComponent != null)
            {
                hasComponent.SetTheme(theme);
            }
        }
        if (returnValue == null)
        {
            foreach (Transform cardSlot in selectedCardSlots)
            {
                CardSlot hasComponent = cardSlot.GetComponent<CardSlot>();
                if (cardSlot.gameObject.activeInHierarchy && hasComponent != null)
                {
                    hasComponent.SetTheme(theme);
                }
            }
        }

        return returnValue;
    }

    #region DebugFunctions

    public void MoveFirstChildFromSelectedToNonSelcted()
    {

        InitCardSlotData();

        foreach (Transform cardSlot in selectedCardSlots)
        {
            if (cardSlot.GetComponent<CardSlot>() != null)
            {
                cardSlot.SetParent(nonSelectedCardSlots.transform);
                break;
            }

            break;
        }
        UpdateSlotsOffset();
        SortNonSelectedCards();
    }

    public void MoveFirstChildFromNonSelctedToSelected()
    {
        InitCardSlotData();
        foreach (Transform cardSlot in nonSelectedCardSlots)
        {
            if (cardSlot.GetComponent<CardSlot>() != null)
            {
                cardSlot.SetParent(selectedCardSlots.transform);
                break;
            }
        }
        UpdateSlotsOffset();
        SortNonSelectedCards();
    }

    #endregion
    #endregion


}
