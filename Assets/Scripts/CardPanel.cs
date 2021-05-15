using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardPanel : MonoBehaviour
{
    [SerializeField] private Transform testPreFab;
    private Transform cardSlots;
    private CardPanelSO currentCardPanelSO;
    private Image background;
    // private CardSlotSO currentActiveCardSlotSO;
    private List<Transform> activeCardSlots;

    private const int paddingOffset = 10;

    public void SetActiveGUI(CardPanelSO cardPanelSO)
    {
        currentCardPanelSO = cardPanelSO;
        UpdateGUI();


    }

    void OnMouseClick()
    {
        Debug.Log("Drag");
    }

    public void AddCardSlot()
    {
        // Transform newCardSlot = Instantiate(testPreFab, cardSlots.transform);
         Transform newCardSlot = Instantiate(currentCardPanelSO.CardSlot.preFab, cardSlots.transform);
       // Debug.Log(newCardSlot.GetComponent<RectTransform>().rect);
        // GameObject newCardSlot = Instantiate(testPreFab);
        // newCardSlot.transform.SetParent(cardSlots.transform);

        //    CardSlots cs = cardSlots.GetComponent<CardSlots>();
        //    cs.CreateSlot(currentCardPanelSO.CardSlot.preFab);
         newCardSlot.GetComponent<CardSlot>().SetActiveGUI(currentCardPanelSO.CardSlot);
          activeCardSlots.Add(newCardSlot);

        if(activeCardSlots.Count == 1)
        {
            UpdatePadding(newCardSlot.GetComponent<RectTransform>());
        }
    }

    private void UpdatePadding(RectTransform rectTransform)
    {
        if (currentCardPanelSO.IsVertical)
        {
            GridLayoutGroup layoutGroup = cardSlots.GetComponent<GridLayoutGroup>();
            int offset = (int)Mathf.Ceil((rectTransform.rect.height / 2.0f)) + paddingOffset;
            layoutGroup.padding.top = offset;
            layoutGroup.padding.bottom = offset;
        }
    }

    private void UpdateGUI()
    {
        background.sprite = currentCardPanelSO.panelBackgroundSprite;
    }

    private void Awake()
    {
        activeCardSlots = new List<Transform>();
        background = transform.Find("background").GetComponent<Image>();
        cardSlots = transform.Find("cardSlotViewport").Find("cardSlots");
    // UpdateGUI();
    }
}
