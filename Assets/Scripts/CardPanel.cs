using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardPanel : MonoBehaviour//, //IDragHandler
{

    [SerializeField] private Transform cardSlotPreFab;

    private const int paddingOffset = 5;

    private GridLayoutGroup cardSlotsGridLayoutGroup;
    private float cardSlotWidth;

    private Transform cardSlots;

    private Transform selectedCardSlots;
    private Transform nonSelectedCardSlots;

    private CardSortOrder sortOrder;

 


    private void Awake()
    {
        sortOrder = CardSortOrder.CardType;
        InitCardSlotData();

    }

    private void Start()
    {
        RemoveAllCardSlots();
        InitNewCards();
        UpdateSlotsOffset();
        SortNonSelectedCards();


        CardPanelThemeSO theme = AssetManager.Instance.GetCardPanelTheme(ThemeManager.Instance.GetCurrentPanelTheme());
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



    public void OnCardClicked(object sender, PanelEventArgs args)
    {
 
        Transform cardSlot = GetCarsSlotInNonSelectedWithCard(args.senderTransform);

        if(cardSlot == null)
        {
            cardSlot = GetCarsSlotInSelectedWithCard(args.senderTransform);
        }


        if(cardSlot != null)
        {
            CardSlot slot = cardSlot.GetComponent<CardSlot>();
     
            if(slot.IsSelected())
            {
                slot.SetIsSelected(false);
                cardSlot.SetParent(nonSelectedCardSlots.transform);
                SortNonSelectedCards();
            }
            else
            {
                slot.SetIsSelected(true);
                cardSlot.SetParent(selectedCardSlots.transform);
            }
            UpdateSlotsOffset();
        }

    }
    public void OnCardDrag(object sender, PanelEventArgs args)
    {
        MoveScrollViewHorizontaly(args.pointerData.delta.x);
    }

    public void OnCardSlotDrag(object sender, PanelEventArgs args)
    {
        MoveScrollViewHorizontaly(args.pointerData.delta.x);
    }

    private void MoveScrollViewHorizontaly(float horizontalVal)
    {
        RectTransform cardSlotTransform = cardSlots.GetComponent<RectTransform>();
        Vector3 newPosition = new Vector3(horizontalVal, 0.0f, 0.0f);
        cardSlotTransform.position = cardSlotTransform.position + newPosition;
    }





    #region CardSlotLogic




    private void InitCardSlotData()
    {

        cardSlots = transform.Find("cardSlotViewport").Find("cardSlots");
        selectedCardSlots = cardSlots.Find("selected");
        nonSelectedCardSlots = cardSlots.Find("nonSelected");

        cardSlotsGridLayoutGroup = cardSlots.GetComponent<GridLayoutGroup>();

        cardSlotWidth = cardSlotPreFab.GetComponent<RectTransform>().rect.width;

        cardSlotsGridLayoutGroup.padding.right = Mathf.CeilToInt(cardSlotWidth / 2.0f) + paddingOffset * 2;
        cardSlotsGridLayoutGroup.padding.left = cardSlotsGridLayoutGroup.padding.right;

        selectedCardSlots.GetComponent<GridLayoutGroup>().spacing = new Vector2(cardSlotWidth + paddingOffset, 0.0f);
        nonSelectedCardSlots.GetComponent<GridLayoutGroup>().spacing = new Vector2(cardSlotWidth + paddingOffset, 0.0f);
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
