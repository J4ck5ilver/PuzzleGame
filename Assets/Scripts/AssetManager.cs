using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{

    public static AssetManager Instance { get; private set; }

    private static Dictionary<CardType, ActionCardSO> actionCardDirectory = null;

    private void Awake()
    {
        LoadActionCardList();
    }

    public Dictionary<CardType, ActionCardSO> GetActionCardDirectory()
    {
        if(actionCardDirectory == null)
        {
            LoadActionCardList();
        }
        return actionCardDirectory;
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
