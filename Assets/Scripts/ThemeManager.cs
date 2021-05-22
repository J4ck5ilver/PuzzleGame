using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{

    public static ThemeManager Instance { private set; get; }


    [SerializeField] CardPanelTheme StartPanelTheme;
    [SerializeField] CardTheme StartCardTheme;


    private CardPanelTheme currentPanelTheme = CardPanelTheme.None;
    private CardTheme currentCardTheme = CardTheme.None;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(currentPanelTheme == CardPanelTheme.None)
        {
            SetPanelTheme(StartPanelTheme);
        }
        if (currentCardTheme == CardTheme.None)
        {
            SetCardTheme(StartCardTheme);
        }
    }

    public void SetCardTheme(CardTheme cardTheme)
    {
        currentCardTheme = StartCardTheme;
        CardManager.Instance.SetTheme(currentCardTheme);
    }

    public void SetPanelTheme(CardPanelTheme panelTheme)
    {
        currentPanelTheme = StartPanelTheme;
        CardPanelManager.Instance.SetTheme(currentPanelTheme);
    }

    public CardPanelTheme GetCurrentPanelTheme()
    {
        return currentPanelTheme;
    }

    public CardTheme GetCurrentCardTheme()
    {
        return currentCardTheme;
    }


}
