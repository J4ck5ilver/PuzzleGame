using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour , IPointerEnterHandler 
{

    private CardDescriptor cardData = new CardDescriptor();

    public void SetData(CardDescriptor descriptor)
    {
        cardData = descriptor;
        UpdateVisuals();
    }
    public CardDescriptor GetData()
    {
        return cardData;
    }

    private void UpdateVisuals()
    {
        TextMeshProUGUI numberOfMovesText = transform.Find("numberOfMovesSprite").Find("text").GetComponent<TextMeshProUGUI>();
        numberOfMovesText.text = cardData.numberOfMoves.ToString();

        if(cardData.direction != Direction.None)
        {
            RectTransform directionTransform = transform.Find("directionSprite").GetComponent<RectTransform>();
            Vector3 rotation = new Vector3(0,0, UtilsClass.DirectionToDegrees(cardData.direction));
            directionTransform.rotation = Quaternion.Euler(rotation);
        }

    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

        
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");
    }
}
