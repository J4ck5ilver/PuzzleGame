using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardPanel : MonoBehaviour
{

    private CardPanelSO cardPanelSO;
    private CardSlotSO cardSlotSO;

    [SerializeField] private Transform cardSlotPreFab;

    private const int paddingOffset = 5;


    private GridLayoutGroup cardSlotsGridLayoutGroup;
    private float cardSlotWidth;

    private Transform selectedCardSlots;
    private Transform nonSelectedCardSlots;




    private void Awake()
    {
        InitData();
    }

    private void Start()
    {
        RemoveAllCardSlots();
        InitNewCards();
    }

    private void InitData()
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

    private void InitNewCards()
    {
       List<Transform> cards = CardManager.Instance.GetCards();

        foreach (Transform card in cards)
        {
            Transform newCardSlot = Instantiate(cardSlotPreFab, nonSelectedCardSlots.transform);
            Transform newCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity, newCardSlot.transform); // Add to cardManager?
            RectTransform rectTrans = newCard.GetComponent<RectTransform>();
            rectTrans.position = new Vector3(0,0,0);
            rectTrans.anchoredPosition = new Vector2(0,0);
            //Update Card UI from ThemeManager
        }
        UpdateSlotsOffset();
        //UpdateUIFunction() // from themeManager
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
            if (cardSlot.gameObject.active)
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
            if (cardSlot.gameObject.active)
            {
                returnValue++;
            }
        }
        return returnValue;
    }


    public void MoveFirstChildFromSelectedToNonSelcted()
    {

        InitData();

        foreach (Transform cardSlot in selectedCardSlots)
        {
            //if (cardSlot.gameObject.active)
            //{

            //    foreach (Transform cardSlotSel in nonSelectedCardSlots)
            //    {
            //        if (cardSlotSel.gameObject.name == cardSlot.name)
            //        {
            //            cardSlot.gameObject.active = false;
            //            cardSlotSel.gameObject.active = true;
            //            break;
            //        }
            //    }
            //    break;
            //}
            if (cardSlot.GetComponent<CardSlot>() != null)
            {
                cardSlot.SetParent(nonSelectedCardSlots.transform);
                //cardSlot.transform.parent = nonSelectedCardSlots.transform;
                break;
            }

            break;
        }
        UpdateSlotsOffset();
    }

    public void MoveFirstChildFromNonSelctedToSelected()
    {
        InitData();
        foreach (Transform cardSlot in nonSelectedCardSlots)
        {
            //if (cardSlot.gameObject.active)
            //{

            //    foreach (Transform cardSlotSel in selectedCardSlots)
            //    {
            //        if (cardSlotSel.gameObject.name == cardSlot.name)
            //        {
            //            cardSlot.gameObject.active = false;
            //            cardSlotSel.gameObject.active = true;
            //            break;
            //        }
            //    }
            //            break;
            //}
            if (cardSlot.GetComponent<CardSlot>() != null)
            {
               // cardSlot.transform.parent = selectedCardSlots.transform;
                cardSlot.SetParent(selectedCardSlots.transform);
                break;
            }

        }
        UpdateSlotsOffset();
        //  print("moveNonToSel");
    }

    private void UpdateSlotsOffset()
    {

        int nrOfSlotsInSelected = GetNumberOfActiveCardSlotsInSelected();
        int nrOfSlotsInNonSelected = GetNumberOfActiveCardSlotsInNonSelected();

        float nonSelectedPanelOffset = ((nrOfSlotsInSelected) * (Mathf.CeilToInt(cardSlotWidth) + paddingOffset)) + (paddingOffset * 2);
        cardSlotsGridLayoutGroup.spacing = new Vector2(nonSelectedPanelOffset, 0.0f);

        int viewOffset = nrOfSlotsInNonSelected * Mathf.CeilToInt((cardSlotWidth + paddingOffset)) -  paddingOffset * 6; //(left, right + middle seperator removal)
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

    //private Transform AddCardSlot()
    //{

    //     Transform newCardSlot = Instantiate(cardSlotPreFab, cardSlots.transform);

    //    newCardSlot.position = new Vector3(0,0,0);
    //    newCardSlot.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
    //    newCardSlot.GetComponent<CardSlot>().SetActiveGUI(cardSlotSO);
    //      activeCardSlots.Add(newCardSlot);

    //    if(activeCardSlots.Count == 1)
    //    {
    //        UpdatePadding(newCardSlot.GetComponent<RectTransform>());
    //    }

    //    return newCardSlot;
    //}

    //private void UpdatePadding(RectTransform rectTransform)
    //{

    //        //GridLayoutGroup layoutGroup = cardSlots.GetComponent<GridLayoutGroup>();
    //        //layoutGroup.SetLayoutVertical();
    //        //int offset = (int)Mathf.Ceil((rectTransform.rect.height / 2.0f)) + paddingOffset;
    //        //layoutGroup.padding.top = offset;
    //        //layoutGroup.padding.bottom = offset;
    //        //layoutGroup.spacing = new Vector2(0, (int)Mathf.Ceil(rectTransform.rect.height) + paddingOffset);

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

    //private void awake()
    //{
    //    activeCardSlots = new List<Transform>();
    //    //cardSlots = transform.Find("cardSlotViewport").Find("cardSlots");

    //}

    //private void Start()
    //{

    //    List<Transform> cards = CardManager.Instance.GetCards();
    //    foreach(Transform card in cards)
    //    {
    //            //instanciate the cards
    //         Transform newCard = Instantiate(card, AddCardSlot());

    //           // card.SetParent(AddCardSlot());
    //        newCard.position = new Vector3(0,0,0);
    //        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    //        card.GetComponent<RectTransform>().position = new Vector3(0,0,0);


    //    }

    //}
}
