using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CardPanelManager : MonoBehaviour
{
    public static CardPanelManager Instance { get; private set; }


    [SerializeField] private Transform startPanel;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        //startPanel = transform.Find("startPanel");
        //endPanel = transform.Find("endPanel");


    }

    public Transform GetStartPanel()
    {
        return startPanel;
    }

    public void SetTheme(CardPanelTheme theme)
    {
        CardPanelThemeSO cardPanelThem = AssetManager.GetCardPanelTheme(theme);
        startPanel.GetComponent<CardPanel>().SetTheme(cardPanelThem);
    }

    public void ResetCardsInPanel()
    {
        startPanel.GetComponent<CardPanel>().SetState(CardPanelState.Reset);
    }

}
