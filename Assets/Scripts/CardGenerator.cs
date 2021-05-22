using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardGenerator : MonoBehaviour
{
 
    [SerializeField] private Transform CardPreFab;
    [SerializeField] private Sprite NonDirectionSprite;
    [SerializeField] private Sprite DirectionSprite;
    [SerializeField] private Vector2 startOffset;

    private const float cardPadding = 5.0f;

    //ActionCardsListSO actionCardsList = null;
    private Dictionary<CardType, ActionCardSO> actionCardDirectory = null;

    private Transform cardManager;
    public void CreateCard(CardDescriptor descriptor)
    {
        LoadData();
        //Transform cardManager = CardManager.Instance.transform;
        cardManager = transform.Find("CardManager").transform;
        Transform newCard = Instantiate(CardPreFab, cardManager);
        newCard.gameObject.name = GetName(descriptor);
        float xOffSet = startOffset.x + (newCard.GetComponent<RectTransform>().rect.width * GetNumbersOfCarsInCardManager()) + cardPadding ;
        newCard.transform.position = new Vector3(xOffSet, startOffset.y, 0);
        SetNonThemeSprites(newCard, descriptor);
        newCard.GetComponent<Card>().SetData(descriptor);
        
    }

    private int GetNumbersOfCarsInCardManager()
    {
        int returnValue = 0;

        foreach(Transform card in cardManager)
        {
            Card hasComponent = card.GetComponent<Card>();
            if(hasComponent != null)
            {
                returnValue++;
            }
        }
        return returnValue;
    }

    private void LoadData()
    {
        if(actionCardDirectory == null)
        {
            actionCardDirectory = new Dictionary<CardType, ActionCardSO>();
            ActionCardsListSO actionCardsList = Resources.Load<ActionCardsListSO>(typeof(ActionCardsListSO).Name);
      
            foreach (ActionCardSO actionCard in actionCardsList.list)
            {
                actionCardDirectory[actionCard.type] = actionCard;
            }
        }
    }

    //Load Sprites before setData on card
    private void SetNonThemeSprites(Transform card, CardDescriptor descriptor)
    {

        Image directionImage = card.Find("directionSprite").GetComponent<Image>();
        if(descriptor.direction == Direction.None)
        {
            directionImage.sprite = NonDirectionSprite;
        }
        else
        {
            directionImage.sprite = DirectionSprite;
        }

        Image actionImage = card.Find("actionSprite").GetComponent<Image>();
        actionImage.sprite = actionCardDirectory[descriptor.type].sprite;

        actionImage.color = Color.white;
        directionImage.color = UtilsClass.GetDirectionColor(descriptor.direction);

    }


    private string GetName(CardDescriptor descriptor)
    {
        


        string returnString = "";
        switch (descriptor.type)
        { 
            case CardType.Walk:
                returnString = descriptor.type.ToString() + " " + descriptor.direction.ToString() + " " + descriptor.numberOfMoves.ToString();
                break;
            case CardType.Jump:
                returnString = descriptor.type.ToString() + " " + descriptor.direction.ToString() + " " + descriptor.numberOfMoves.ToString();
                break;
            case CardType.Roll:
                returnString = descriptor.type.ToString() + " " + descriptor.direction.ToString() + " " + descriptor.numberOfMoves.ToString();
                break;
            default:
                returnString = "Not a valid card";
            break;

        }


        return returnString;
    }

}
