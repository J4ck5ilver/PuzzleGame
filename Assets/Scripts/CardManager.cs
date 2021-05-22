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
        InitStartCards();
    }

    private void InitStartCards()
    {
        foreach (Transform childCard in transform)
        {
            Card hasCompnent = childCard.GetComponent<Card>();
            if (hasCompnent != null)
            {
                cards.Add(childCard);
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
        CardThemeSO cardTheme = AssetManager.Instance.GetCardTheme(theme);

        // Loop all cards here
    }

    public List<Transform> GetCards()
    {
        return cards;
    }




}
