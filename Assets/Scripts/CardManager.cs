using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardManager : MonoBehaviour
{

    public static CardManager Instance { get; private set; }

    private List<Transform> startCards = new List<Transform>();

    private List<Transform> cardsInPlay = new List<Transform>();

    private Direction OrientationDirection = Direction.South;

    private Transform currentCardInPlay;

    private float cardRotationOffset = 0.0f;
    private void Awake()
    {
        Instance = this;
        InitStartCards();
        CameraManager.Instance.OnChangedLatitudeEvent += OnLatiduteChange;
    }

    private void OnLatiduteChange(object sender, LatitudeChangeArgs args)
    {

        if(args.Latitude == OrientationDirection)
        {
            cardRotationOffset = 0.0f;
        }
        else 
        {
            if(OrientationDirection == Direction.North)
            {
                Debug.Log("North Direction Not Implemented");
            }
            else if (OrientationDirection == Direction.East)
            {
                Debug.Log("East Direction Not Implemented");
            }
            else if (OrientationDirection == Direction.South)
            {

                if (args.Latitude == Direction.North)
                {
                    cardRotationOffset = 180.0f;
                }
                else if (args.Latitude == Direction.East)
                {
                    cardRotationOffset = -90.0f;
                }
                else
                {
                    cardRotationOffset = 90.0f;
                }
            }
            else if (OrientationDirection == Direction.West)
            {
                Debug.Log("West Direction Not Implemented");
            }
        }

        foreach (Transform card in cardsInPlay)
        {
            card.GetComponent<Card>().SetDirectionArrowRotationOffset(cardRotationOffset);
        }
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
