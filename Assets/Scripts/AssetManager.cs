using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{

    public static AssetManager Instance { get; private set; }

    private static Dictionary<CardType, ActionCardSO> actionCardDirectory = null;
    private static Dictionary<CardTheme, CardThemeSO> cardThemeDirectory = null;
    private static Dictionary<CardPanelTheme, CardPanelThemeSO> cardPanelThemeDirectory = null;

    private void Awake()
    {
        Instance = this;
        LoadActionCardList();
        LoadPanelThemeList();
        LoadCardThemeList();
    }

    public CardThemeSO GetCardTheme(CardTheme theme)
    {
        CardThemeSO returnValue;
        if (cardThemeDirectory.TryGetValue(theme, out returnValue))
        {
            return returnValue;
        }
        return null;
    }

    public CardPanelThemeSO GetCardPanelTheme(CardPanelTheme theme)
    {
        CardPanelThemeSO returnValue;
        if (cardPanelThemeDirectory.TryGetValue(theme,out returnValue))
        {
            return returnValue;
        }
        return null;
    }

    public Dictionary<CardType, ActionCardSO> GetActionCardDirectory()
    {
        if(actionCardDirectory == null)
        {
            LoadActionCardList();
        }
        return actionCardDirectory;
    }

    private void LoadPanelThemeList()
    {
        cardPanelThemeDirectory =   new Dictionary<CardPanelTheme, CardPanelThemeSO>();
        CardPanelThemeListSO cardPanelThemeList = Resources.Load<CardPanelThemeListSO>(typeof(CardPanelThemeListSO).Name);

        foreach (CardPanelThemeSO cardPanelTheme in cardPanelThemeList.list)
        {
            cardPanelThemeDirectory[cardPanelTheme.theme] = cardPanelTheme;
        }
    }
    private void LoadCardThemeList()
    {
        cardThemeDirectory = new Dictionary<CardTheme, CardThemeSO>();
        CardThemeListSO cardThemeList = Resources.Load<CardThemeListSO>(typeof(CardThemeListSO).Name);

        foreach (CardThemeSO cardTheme in cardThemeList.list)
        {
            cardThemeDirectory[cardTheme.theme] = cardTheme;
        }
    }

    private void LoadActionCardList()
    {

        actionCardDirectory = new Dictionary<CardType, ActionCardSO>();
        ActionCardsListSO actionCardsList = Resources.Load<ActionCardsListSO>(typeof(ActionCardsListSO).Name);

        foreach (ActionCardSO actionCard in actionCardsList.list)
        {
            actionCardDirectory[actionCard.type] = actionCard;
        }

    }
}
