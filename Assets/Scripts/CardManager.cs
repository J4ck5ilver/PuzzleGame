using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public static CardManager Instance { get; private set; }

    private List<Transform> cards = new List<Transform>();

    private void Awake()
    {
        
        Instance = this;

        foreach(Transform childCard in transform)
        {
            Card hasCompnent = childCard.GetComponent<Card>();
            if(hasCompnent != null)
            {
                cards.Add(childCard);
            }
        }

    }

    public void SetTheme(CardTheme theme)
    {
        CardThemeSO cardTheme = AssetManager.Instance.GetCardTheme(theme);

        // Loop all cards here
    }

    public List<Transform> GetCards()
    {
        return cards;
    }




}
