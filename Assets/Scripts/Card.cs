using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour, IPointerEnterHandler
{

    [SerializeField] private int numberOfMoves;
    [SerializeField] private CardType cardType;
    [SerializeField] private Direction direction;

    public void SetData(CardDescriptor descriptor)
    {

        numberOfMoves = descriptor.numberOfMoves;
        cardType = descriptor.type;
        direction = descriptor.direction;
        UpdateVisuals();


        //// todo add dir + non dir sprite into assetmanager and get them from there
        //Dictionary<CardType, ActionCardSO> actionCards = AssetManager.Instance.GetActionCardDirectory();

        //Image directionImage = transform.Find("directionSprite").GetComponent<Image>();
        //if (descriptor.direction == Direction.None)
        //{
        //    directionImage.sprite = NonDirectionSprite;
        //}
        //else
        //{
        //    directionImage.sprite = DirectionSprite;
        //}

        //Image actionImage = card.Find("actionSprite").GetComponent<Image>();
        //actionImage.sprite = actionCardDirectory[descriptor.type].sprite;

        //actionImage.color = Color.white;
        //directionImage.color = UtilsClass.GetDirectionColor(descriptor.direction);





    }

    private void UpdateVisuals()
    {
        TextMeshProUGUI numberOfMovesText = transform.Find("numberOfMovesSprite").Find("text").GetComponent<TextMeshProUGUI>();
        numberOfMovesText.text = Mathf.Clamp(numberOfMoves, GameConstants.minNumberOfMoves, GameConstants.maxNumberOfMoves).ToString();

        if (direction != Direction.None)
        {
            RectTransform directionTransform = transform.Find("directionSprite").GetComponent<RectTransform>();
            Vector3 rotation = new Vector3(0, 0, UtilsClass.DirectionToDegrees(direction));
            directionTransform.rotation = Quaternion.Euler(rotation);
        }

    }
    public CardDescriptor GetData()
    {
        //if (cardData == null)
        //{


        //    cardData = new CardDescriptor();

        //    TextMeshProUGUI numberOfMovesText = transform.Find("numberOfMovesSprite").Find("text").GetComponent<TextMeshProUGUI>();
        //    cardData.numberOfMoves = int.Parse(numberOfMovesText.text.ToString());

        //    RectTransform directionTransform = transform.Find("directionSprite").GetComponent<RectTransform>();
        //    cardData.direction = UtilsClass.DegreesToDirection(directionTransform.transform.rotation.eulerAngles.z);

        //    Dictionary<CardType, ActionCardSO> actionCards =  AssetManager.Instance.GetActionCardDirectory();

        //    var cardTypes = CardType.GetValues(typeof(CardType));

        //    foreach(CardType type in cardTypes)
        //    {
        //        actionCards[type].sprite;
        //    }


        //}

        CardDescriptor cardData = new CardDescriptor();



        cardData.numberOfMoves = numberOfMoves;
        cardData.type          = cardType;
        cardData.direction     = direction;

        return cardData;
    }

    public void SetTheme(CardThemeSO theme)
    {
        if (theme != null)
        {

        }
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {


        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");
    }
}
