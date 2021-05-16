using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardGenerator : MonoBehaviour
{
 
    [SerializeField] public Transform CardPreFab;
    [SerializeField] public Sprite NonDirectionSprite;
    [SerializeField] public Sprite DirectionSprite;


    //ActionCardsListSO actionCardsList = null;
    private Dictionary<CardType, ActionCardSO> actionCardDirectory = null;


    public void CreateCard(CardDescriptor descriptor)
    {
        LoadData();
        //Transform cardManager = CardManager.Instance.transform;
        Transform cardManager = transform.Find("CardManager").transform;
        Transform newCard = Instantiate(CardPreFab, cardManager);
        newCard.gameObject.name = GetName(descriptor);

        SetNonThemeSprites(newCard, descriptor);
        newCard.GetComponent<Card>().SetData(descriptor);
        


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
