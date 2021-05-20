using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardPanel : MonoBehaviour
{

    [SerializeField] private Transform cardSlotPreFab;

    private const int paddingOffset = 5;

    private GridLayoutGroup cardSlotsGridLayoutGroup;
    private float cardSlotWidth;

    private Transform selectedCardSlots;
    private Transform nonSelectedCardSlots;

    private CardSortOrder sortOrder;


    private void Awake()
    {
        // SortOrder = new CardSortOrder();
        sortOrder = CardSortOrder.CardType;
        InitCardSlotData();

    }

    private void Start()
    {
        RemoveAllCardSlots();
        InitNewCards();
        UpdateSlotsOffset();
        SortNonSelectedCards();
        //UpdateUIFunction() // from themeManager
    }

    public void SetTheme(CardPanelThemeSO theme)
    {
        if(theme != null)
        {
            transform.Find("background").GetComponent<Image>().sprite = theme.panelBackgroundSprite;
        }
    }

    private void InitCardSlotData()
    {

        Transform cardSlots = transform.Find("cardSlotViewport").Find("cardSlots");
        selectedCardSlots = cardSlots.Find("selected");
        nonSelectedCardSlots = cardSlots.Find("nonSelected");

        cardSlotsGridLayoutGroup = cardSlots.GetComponent<GridLayoutGroup>();

        cardSlotWidth = cardSlotPreFab.GetComponent<RectTransform>().rect.width;

        cardSlotsGridLayoutGroup.padding.right = Mathf.CeilToInt(cardSlotWidth / 2.0f) + paddingOffset * 2;
        cardSlotsGridLayoutGroup.padding.left = cardSlotsGridLayoutGroup.padding.right;

        selectedCardSlots.GetComponent<GridLayoutGroup>().spacing = new Vector2(cardSlotWidth + paddingOffset, 0.0f);
        nonSelectedCardSlots.GetComponent<GridLayoutGroup>().spacing = new Vector2(cardSlotWidth + paddingOffset, 0.0f);
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
                        for (int i = GameConstants.minNumberOfMoves; i < GameConstants.maxNumberOfMoves; i++)
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
                        for (int i = GameConstants.minNumberOfMoves; i < GameConstants.maxNumberOfMoves; i++)
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


    private void InitNewCards()
    {
        List<Transform> cards = CardManager.Instance.GetCards();

        foreach (Transform card in cards)
        {
            Transform newCardSlot = Instantiate(cardSlotPreFab, nonSelectedCardSlots.transform);
            RectTransform rectTransCardSlot = newCardSlot.GetComponent<RectTransform>();
            rectTransCardSlot.position = new Vector3(0, 0, 0);
            rectTransCardSlot.anchoredPosition = new Vector2(0, 0);

            CardDescriptor test = card.GetComponent<Card>().GetData();

            Transform newCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity, newCardSlot.transform); // Add to cardManager?
            newCard.GetComponent<Card>().SetData(card.GetComponent<Card>().GetData());
            RectTransform rectTransCard = newCard.GetComponent<RectTransform>();
            rectTransCard.position = new Vector3(0, 0, 0);
            rectTransCard.anchoredPosition = new Vector2(0, 0);
            //Update Card UI from ThemeManager
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

    private void UpdateSlotsOffset()
    {

        int nrOfSlotsInSelected = GetNumberOfActiveCardSlotsInSelected();
        int nrOfSlotsInNonSelected = GetNumberOfActiveCardSlotsInNonSelected();

        float nonSelectedPanelOffset = ((nrOfSlotsInSelected) * (Mathf.CeilToInt(cardSlotWidth) + paddingOffset)) + (paddingOffset * 2);
        cardSlotsGridLayoutGroup.spacing = new Vector2(nonSelectedPanelOffset, 0.0f);

        int viewOffset = nrOfSlotsInNonSelected * Mathf.CeilToInt((cardSlotWidth + paddingOffset)) - paddingOffset * 6; //(left, right + middle seperator removal)
        cardSlotsGridLayoutGroup.padding.left = viewOffset;

    }

    //public void SetPanelGUI(CardPanelSO cardPanelSO)
    //{
    //    this.cardPanelSO = cardPanelSO;
    //    UpdatePanelGUI();
    //}

    //public void SetCardSlotGUI(CardSlotSO cardSlotSO)
    //{
    //    this.cardSlotSO = cardSlotSO;
    //    UpdateCardSlotGUI();
    //}

    //void OnMouseClick()
    //{
    //    Debug.Log("Drag");
    //}

    //private void UpdatePanelGUI()
    //{
    //    Image backgroundImage = transform.Find("background").GetComponent<Image>();
    //    backgroundImage.sprite = cardPanelSO.panelBackgroundSprite;
    //}

    //private void UpdateCardSlotGUI()
    //{

    //   foreach(Transform card in activeCardSlots)
    //    {
    //        card.GetComponent<CardSlot>().SetActiveGUI(cardSlotSO);
    //    }

    //    Image backgroundImage = transform.Find("background").GetComponent<Image>();
    //    backgroundImage.sprite = cardPanelSO.panelBackgroundSprite;
    //}

}
