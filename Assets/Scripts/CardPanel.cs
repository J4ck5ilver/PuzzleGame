using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardPanel : MonoBehaviour, IDragHandler
{

    public delegate void Play();
    public static event Play OnPlay;



    [SerializeField] private Transform cardSlotPreFab;
    private const float holdCardDelay = 0.25f;
    private const float holdCardPanelScrollSpeed = 6.0f;
    private const int paddingOffset = 5;
    private const float separatorXOffset = paddingOffset * 1.5f;
    private const int panelSidesOffset = paddingOffset * 2;


    private int lastHoldCardSelectionGroupIndex = -1;

    private GridLayoutGroup cardSlotsGridLayoutGroup;
    private float cardSlotWidth;

    private Transform selectionCardSlots;

    private Transform holdingCardCardSlot = null;

    private Transform currentCardInPlay;

    private Transform outputCardSlot;
    private Transform selectedCardSlots;
    private Transform nonSelectedCardSlots;
    private Transform movingCardSeparator;

    private Transform playButtonTransform;

    private CardSortOrder sortOrder;

    private CardPanelState currentState;

    private bool scrollingEnabled = true;
    private bool carSelectionEnabled = true;

    private float deltaFloat = 0.0f;

    private Dictionary<int, List<Transform>> playedCardSlots;

    private int currentRound;

    private void Awake()
    {
        sortOrder = CardSortOrder.CardType;
        currentState = CardPanelState.Select;
        InitCardSlotData();

        playedCardSlots = new Dictionary<int, List<Transform>>();

        playButtonTransform = transform.Find("playButton");
        PlayButton component = playButtonTransform.GetComponent<PlayButton>();
        component.OnClick += OnPlayButtonClicked;

        currentRound = 0;
    }

    private void OnNewRound()
    {
        SetState(CardPanelState.NextStep);
    }

    private void OnNewTurn()
    {
        NextCard();
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
        GameManager.OnNewRound += OnNewRound;
        GameManager.OnTurnCompleted += OnNewTurn;
    }

    private void Update()
    {
        UpdateHoldingCardVisuals();
    }

    private void UpdateHoldingCardVisuals()
    {
        if (holdingCardCardSlot != null)
        {
            Card tmpCard = holdingCardCardSlot.GetComponent<CardSlot>().GetCard().GetComponent<Card>();
            if (tmpCard != null)
            {
                deltaFloat += Time.deltaTime * 1.8f;
                float alfaVal = Mathf.Lerp(0.3f, 0.6f, Mathf.Abs(Mathf.Sin(deltaFloat)));
                tmpCard.SetAlfa(alfaVal);
            }

        }
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
        if (carSelectionEnabled && holdingCardCardSlot == null)
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

    private void OnCardHoldDestroyed(object sender, EventArgs e)
    {

        CardSlot cardSlot = holdingCardCardSlot.GetComponent<CardSlot>();
        cardSlot.GetCard().gameObject.SetActive(true);
        cardSlot.GetCard().GetComponent<Card>().SetAlfa(1.0f);

        if (holdingCardCardSlot.gameObject.activeInHierarchy)
        {
            int jj = 0;
        }
        else
        {
            holdingCardCardSlot.gameObject.SetActive(true);



            cardSlot.SetIsSelected(false);
            cardSlot.transform.SetParent(nonSelectedCardSlots.transform);

            SortNonSelectedCards();
            UpdateSlotsOffset();
            UpdateMovingCardSeparator();

        }


        holdingCardCardSlot = null;
        scrollingEnabled = true;
        lastHoldCardSelectionGroupIndex = -1;


    }

    private void OnCardHold(object sender, PanelEventArgs e)
    {
        if (currentState != CardPanelState.Play && holdingCardCardSlot == null)
        {
            Transform canvasTransform = UtilsClass.GetTopmostCanvas(this).transform;
            Card tmpCard = Instantiate(e.senderTransform, canvasTransform).GetComponent<Card>();
            tmpCard.transform.position = UtilsClass.GetMouseWorldPosition();


            holdingCardCardSlot = GetCarsSlotThatContainsCard(e.senderTransform);
            CardSlot cardSlot = holdingCardCardSlot.GetComponent<CardSlot>();

            cardSlot.GetCard().gameObject.SetActive(true);


            if (!cardSlot.IsSelected())
            {
                holdingCardCardSlot.gameObject.SetActive(false);
                SortNonSelectedCards();
                UpdateSlotsOffset();
            }

            FollowMouse followMouse = tmpCard.gameObject.AddComponent<FollowMouse>();
            followMouse.OnDestroyed += OnCardHoldDestroyed;
            followMouse.HoldObjectiveOnRightSideOfScreen += OnCardRightOfScreen;
            followMouse.HoldObjectiveOnLeftSideOfScreen += OnCardLeftOfScreen;
            scrollingEnabled = false;
        }
    }

    public void OnHoldCardEnterSelectionGroup(object sender, PanelEventArgs arg)
    {

        CardSlot cardSlot = holdingCardCardSlot.GetComponent<CardSlot>();

        if (!cardSlot.IsSelected())
        {

            cardSlot.SetIsSelected(true);
            cardSlot.transform.SetParent(selectedCardSlots.transform);

            lastHoldCardSelectionGroupIndex = arg.intData;
            holdingCardCardSlot.SetSiblingIndex(lastHoldCardSelectionGroupIndex);
            holdingCardCardSlot.gameObject.SetActive(true);

            UpdateMovingCardSeparator();
            UpdateSlotsOffset();
        }
        else
        {
            lastHoldCardSelectionGroupIndex = holdingCardCardSlot.GetSiblingIndex();
        }
    }

    public void OnHoldCardExitSelectionGroup(object sender, EventArgs args)
    {
        CardSlot cardSlot = holdingCardCardSlot.GetComponent<CardSlot>();

        holdingCardCardSlot.gameObject.SetActive(false);
        cardSlot.SetIsSelected(false);
        cardSlot.transform.SetParent(nonSelectedCardSlots.transform);
        lastHoldCardSelectionGroupIndex = -1;

        UpdateSlotsOffset();
        UpdateMovingCardSeparator();
    }

    private void OnHoldCardCollidingWithSelectedCardGroup(object sender, PanelEventArgs arg)
    {
        int index = arg.intData;

        if (lastHoldCardSelectionGroupIndex != index)
        {
            holdingCardCardSlot.SetSiblingIndex(index);
            UpdateMovingCardSeparator();
            lastHoldCardSelectionGroupIndex = index;
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

    private void OnCardLeftOfScreen(object sender, PanelEventArgs arg)
    {
        float interpolatedScrollSpeed = Mathf.Lerp(0.0f, holdCardPanelScrollSpeed, arg.floatData);
        MoveScrollViewHorizontaly(interpolatedScrollSpeed, true);
    }
    private void OnCardRightOfScreen(object sender, PanelEventArgs arg)
    {
        float interpolatedScrollSpeed = Mathf.Lerp(0.0f, holdCardPanelScrollSpeed, arg.floatData);
        MoveScrollViewHorizontaly(-interpolatedScrollSpeed, true);
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

    private void MoveScrollViewHorizontaly(float horizontalVal, bool Override = false)
    {
        if ((Override || scrollingEnabled) && currentState != CardPanelState.Play)
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
        playButtonTransform.GetComponent<PlayButton>().enabled = state;
        playButtonTransform.gameObject.SetActive(state);
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

        SelectedCardSlotGroup selectedCardSlotsComponent = selectedCardSlots.GetComponent<SelectedCardSlotGroup>();

        selectedCardSlotsComponent.CollisionStayEvent += OnHoldCardCollidingWithSelectedCardGroup;
        selectedCardSlotsComponent.CollisionEnterEvent += OnHoldCardEnterSelectionGroup;
        selectedCardSlotsComponent.CollisionExitEvent += OnHoldCardExitSelectionGroup;



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

                if (currentCardInPlay != null)
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

        if (noMoreCards && currentCardInPlay != null)
        {
            Destroy(currentCardInPlay.gameObject);
            currentCardInPlay = null;
        }

        UpdateSlotsOffset();
        UpdateMovingCardSeparator();
        CardManager.Instance.SetCurrentCardInPlay(currentCardInPlay);

    }

    private void UpdateCardSlots()
    {
        SortNonSelectedCards();
        UpdateSlotsOffset();
        SortNonSelectedCards();
    }

    private void EnableUserInterraction(bool state)
    {
        EnableScrolling(state);
        EnablePlayButton(state);
        EnableCardSelection(state);
    }

    public void SetState(CardPanelState state)
    {
        currentState = state;

        switch (state)
        {
            case CardPanelState.Play:

                //Vector3 oldPosition = selectionCardSlots.GetComponent<RectTransform>().position;
                EnableUserInterraction(false);
                NextCard();
                //selectionCardSlots.GetComponent<RectTransform>().position = new Vector3(0.0f, oldPosition.y, oldPosition.z);
                // UpdateSlotsOffset();

                Vector3 oldPosition = selectionCardSlots.GetComponent<RectTransform>().position;
                RectTransform rectTransform = selectionCardSlots.GetComponent<RectTransform>();
                rectTransform.position = new Vector3(0.0f, oldPosition.y, oldPosition.z);
                rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);

                rectTransform.anchoredPosition3D = new Vector3(0.0f, 0.0f, 0.0f);

                if (OnPlay != null)
                {
                    OnPlay();
                }


                rectTransform.anchoredPosition3D = new Vector3(0.0f, 0.0f, 0.0f);

                break;

            case CardPanelState.HardReset:
                EnableUserInterraction(true);
                MoveAllCardSlotsInSelectedGroupToNonSelected();
                ActivateAllDisabledCardSlotsInNonSelectedGroup();
                MoveCardsSlotsInPlayedListToSelected();
                playedCardSlots = new Dictionary<int, List<Transform>>();
                UpdateCardSlots();
                    currentRound = 0;
                break;


            case CardPanelState.SoftReset:

                EnableUserInterraction(true);

                if (playedCardSlots.Count != 0)
                {
                    currentRound--;
                    MoveAllCardSlotsInSelectedGroupToNonSelected();
                    MoveCardsSlotsInPlayedListToSelectedFromCurrentTurn();
                    ActivateAllDisabledCardSlotsInSelectedGroup();
                    UpdateCardSlots();
                    playedCardSlots.Remove(currentRound);
                    if (currentRound < 0)
                    {
                        currentRound = 0;
                    }
                }
                break;

            case CardPanelState.Select:


                break;
            case CardPanelState.NextStep:
                EnableUserInterraction(true);
                AddDisabledCardSlotsToPlayedList();
                MoveAllDisabledSlotsInSelectedGroupToNonSelected();
                UpdateCardSlots();
                SetState(CardPanelState.Select);
                currentRound++;

                break;
        }
    }

    private void MoveCardsSlotsInPlayedListToSelectedFromCurrentTurn()
    {
        foreach (Transform cardSlot in playedCardSlots[currentRound])
        {
            cardSlot.GetComponent<CardSlot>().SetIsSelected(true);
            cardSlot.transform.SetParent(selectedCardSlots.transform);
        }
    }
    private void MoveCardsSlotsInPlayedListToSelected()
    {
        foreach (int turn in playedCardSlots.Keys)
        {
            foreach (Transform cardSlot in playedCardSlots[turn])
            {
                cardSlot.GetComponent<CardSlot>().SetIsSelected(true);
                cardSlot.transform.SetParent(selectedCardSlots.transform);
            }
        }
    }
    private void AddDisabledCardSlotsToPlayedList()
    {
        playedCardSlots.Add(currentRound, new List<Transform>());


        foreach (Transform cardSlot in selectedCardSlots)
        {
            if (!cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>() != null)
            {
                playedCardSlots[currentRound].Add(cardSlot);
            }
        }


    }

    private void MoveAllCardSlotsInSelectedGroupToNonSelected()
    {

        List<Transform> slotsToMove = new List<Transform>();

        foreach (Transform cardSlot in selectedCardSlots)
        {
            if (cardSlot.GetComponent<CardSlot>() != null)
            {
                slotsToMove.Add(cardSlot);
            }
        }

        foreach (Transform cardSlot in slotsToMove)
        {
            cardSlot.GetComponent<CardSlot>().SetIsSelected(false);
            cardSlot.transform.SetParent(nonSelectedCardSlots.transform);
        }
    }

    private void ActivateAllDisabledCardSlotsInNonSelectedGroup()
    {
        foreach (Transform cardSlot in nonSelectedCardSlots)
        {
            if (!cardSlot.gameObject.activeInHierarchy && cardSlot.GetComponent<CardSlot>() != null)
            {
                cardSlot.gameObject.SetActive(true);
            }
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

    }

    private void ActivateAllDisabledCardSlotsInSelectedGroup()
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
            cardComponent.SetCardHoldDelay(holdCardDelay);
            cardComponent.CardDragEvent += OnCardDrag;
            cardComponent.CardHoldCardEvent += OnCardHold;
            cardComponent.CardClickedEvent += OnCardClicked;

            RectTransform rectTransCard = newCard.GetComponent<RectTransform>();
            rectTransCard.position = new Vector3(0, 0, 0);
            rectTransCard.anchoredPosition = new Vector2(0, 0);
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

        float nonSelectedPanelOffset = ((nrOfSlotsInSelected) * (Mathf.CeilToInt(cardSlotWidth) + paddingOffset)) + panelSidesOffset;
        cardSlotsGridLayoutGroup.spacing = new Vector2(nonSelectedPanelOffset, 0.0f);

        BoxCollider2D selectedGroupBoxCollider = selectedCardSlots.GetComponent<BoxCollider2D>();

        Vector2 hitboxSize = new Vector2((paddingOffset + ((cardSlotWidth + paddingOffset) * nrOfSlotsInSelected)), cardSlotPreFab.GetComponent<RectTransform>().rect.height + panelSidesOffset);

        selectedGroupBoxCollider.size = hitboxSize;

        selectedGroupBoxCollider.offset = new Vector2(((-selectedGroupBoxCollider.size.x / 2.0f) + (cardSlotWidth / 2.0f) + paddingOffset), 0.0f);




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

            float xOffset = (cardSlottransform.GetComponent<CardSlot>().GetWidth() / 2.0f) + separatorXOffset;

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

    private Transform GetCardSlotInSelectedWithCard(Transform cardToFind)
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

    private Transform GetCarsSlotThatContainsCard(Transform cardToFind)
    {
        Transform returnValue = GetCardSlotInSelectedWithCard(cardToFind);

        if (returnValue == null)
        {
            returnValue = GetCarsSlotInNonSelectedWithCard(cardToFind);
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
    #endregion


}
