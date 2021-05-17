using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardPanel : MonoBehaviour
{
    private Transform cardSlots;
    private List<Transform> activeCardSlots;

    private CardPanelSO cardPanelSO;
    private CardSlotSO cardSlotSO;

    [SerializeField] private Transform cardSlotPreFab;

    private const int paddingOffset = 5;
    public void SetPanelGUI(CardPanelSO cardPanelSO)
    {
        this.cardPanelSO = cardPanelSO;
        UpdatePanelGUI();
    }

    public void SetCardSlotGUI(CardSlotSO cardSlotSO)
    {
        this.cardSlotSO = cardSlotSO;
        UpdateCardSlotGUI();
    }

    void OnMouseClick()
    {
        Debug.Log("Drag");
    }

    private Transform AddCardSlot()
    {

         Transform newCardSlot = Instantiate(cardSlotPreFab, cardSlots.transform);

        newCardSlot.position = new Vector3(0,0,0);
        newCardSlot.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
        newCardSlot.GetComponent<CardSlot>().SetActiveGUI(cardSlotSO);
          activeCardSlots.Add(newCardSlot);

        if(activeCardSlots.Count == 1)
        {
            UpdatePadding(newCardSlot.GetComponent<RectTransform>());
        }

        return newCardSlot;
    }

    private void UpdatePadding(RectTransform rectTransform)
    {
 
            GridLayoutGroup layoutGroup = cardSlots.GetComponent<GridLayoutGroup>();
            layoutGroup.SetLayoutVertical();
            int offset = (int)Mathf.Ceil((rectTransform.rect.height / 2.0f)) + paddingOffset;
            layoutGroup.padding.top = offset;
            layoutGroup.padding.bottom = offset;
            layoutGroup.spacing = new Vector2(0, (int)Mathf.Ceil(rectTransform.rect.height) + paddingOffset);
        
    }

    private void UpdatePanelGUI()
    {
        Image backgroundImage = transform.Find("background").GetComponent<Image>();
        backgroundImage.sprite = cardPanelSO.panelBackgroundSprite;
    }

    private void UpdateCardSlotGUI()
    {

       foreach(Transform card in activeCardSlots)
        {
            card.GetComponent<CardSlot>().SetActiveGUI(cardSlotSO);
        }

        Image backgroundImage = transform.Find("background").GetComponent<Image>();
        backgroundImage.sprite = cardPanelSO.panelBackgroundSprite;
    }

    private void Awake()
    {
        activeCardSlots = new List<Transform>();
        cardSlots = transform.Find("cardSlotViewport").Find("cardSlots");

    }

    private void Start()
    {
     
        List<Transform> cards = CardManager.Instance.GetCards();
        foreach(Transform card in cards)
        {
                //instanciate the cards
             Transform newCard = Instantiate(card, AddCardSlot());

               // card.SetParent(AddCardSlot());
            newCard.position = new Vector3(0,0,0);
            card.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            card.GetComponent<RectTransform>().position = new Vector3(0,0,0);
              
            
        }

    }
}
