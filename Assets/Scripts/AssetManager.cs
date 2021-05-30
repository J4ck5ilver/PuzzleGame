using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetManager
{


    [SerializeField] private static Dictionary<CardType, ActionCardSO> actionCardDirectory = null;
    [SerializeField] private static Dictionary<CardTheme, CardThemeSO> cardThemeDirectory = null;
    [SerializeField] private static Dictionary<CardPanelTheme, CardPanelThemeSO> cardPanelThemeDirectory = null;


    public static CardThemeSO GetCardTheme(CardTheme theme)
    {
        if (actionCardDirectory == null)
        {
            LoadCardThemeList();
        }

        CardThemeSO returnValue;
        if (cardThemeDirectory.TryGetValue(theme, out returnValue))
        {
            return returnValue;
        }
        return null;
    }

    public static CardPanelThemeSO GetCardPanelTheme(CardPanelTheme theme)
    {

        if (cardPanelThemeDirectory == null)
        {
            LoadPanelThemeList();
        }
        CardPanelThemeSO returnValue;
        if (cardPanelThemeDirectory.TryGetValue(theme, out returnValue))
        {
            return returnValue;
        }
        return null;
    }

    public static Dictionary<CardType, ActionCardSO> GetActionCardDirectory()
    {
        if (actionCardDirectory == null)
        {
            LoadActionCardList();
        }
        return actionCardDirectory;
    }

    private static void LoadPanelThemeList()
    {

        cardPanelThemeDirectory = new Dictionary<CardPanelTheme, CardPanelThemeSO>();
        CardPanelThemeListSO cardPanelThemeList = Resources.Load<CardPanelThemeListSO>(typeof(CardPanelThemeListSO).Name);

        foreach (CardPanelThemeSO cardPanelTheme in cardPanelThemeList.list)
        {
            cardPanelThemeDirectory[cardPanelTheme.theme] = cardPanelTheme;
        }
    }
    private static void LoadCardThemeList()
    {
        cardThemeDirectory = new Dictionary<CardTheme, CardThemeSO>();
        CardThemeListSO cardThemeList = Resources.Load<CardThemeListSO>(typeof(CardThemeListSO).Name);

        foreach (CardThemeSO cardTheme in cardThemeList.list)
        {
            cardThemeDirectory[cardTheme.theme] = cardTheme;
        }
    }

    private static void LoadActionCardList()
    {

        actionCardDirectory = new Dictionary<CardType, ActionCardSO>();
        ActionCardsListSO actionCardsList = Resources.Load<ActionCardsListSO>(typeof(ActionCardsListSO).Name);

        foreach (ActionCardSO actionCard in actionCardsList.list)
        {
            actionCardDirectory[actionCard.type] = actionCard;
        }

    }
}
