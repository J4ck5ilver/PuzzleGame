using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public static CardManager Instance { get; private set; }

    private List<Transform> startCards = new List<Transform>();

    private List<Transform> cardsInPlay = new List<Transform>();

    private Transform currentCardInPlay;
    private void Awake()
    {
        Instance = this;
        InitStartCards();
    }

    private void InitStartCards()
    {
        foreach (Transform childCard in transform)
        {
            Card hasCompnent = childCard.GetComponent<Card>();
            if (hasCompnent != null)
            {
                startCards.Add(childCard);
            }
        }
    }

    public void RemoveAllStartCards()
    {
        List<Transform> childsToDestroy = new List<Transform>();

        foreach (Transform childCard in transform)
        {
            Card hasCompnent = childCard.GetComponent<Card>();
            if (hasCompnent != null)
            {
                childsToDestroy.Add(childCard);
             
            }
        }
        foreach (Transform child in childsToDestroy)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public void SetTheme(CardTheme theme)
    {
        CardThemeSO cardTheme = AssetManager.GetCardTheme(theme);
        

        foreach (Transform startCard in startCards)
        {
            startCard.GetComponent<Card>().SetTheme(cardTheme);
        }

        foreach(Transform cardInPlay in cardsInPlay)
        {
            cardInPlay.GetComponent<Card>().SetTheme(cardTheme);
        }

    }
    public void  AddToCardsInPlay(Transform cardToAdd)
    {
        cardsInPlay.Add(cardToAdd);
    }

    public void SetCurrentCardInPlay(Transform cardToAdd)
    {
        currentCardInPlay = cardToAdd;
    }

    public List<Transform> GetStartCards()
    {
        return startCards;
    }
    public List<Transform> GetCardsInPlay()
    {
        return cardsInPlay;
    }

    public Card GetCurrentCardInPlay()
    {
        Card returnCard = null;
        if(currentCardInPlay != null)
        {
            returnCard = currentCardInPlay.GetComponent<Card>();
        }

        return returnCard;
    }



}
