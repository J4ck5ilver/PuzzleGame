using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/CardPanel")]
public class CardPanelSO : ScriptableObject
{
    [SerializeField] public CardSlotSO CardSlot;

    public Transform preFab;
    public Sprite panelBackgroundSprite;
    public Sprite panelBorderSprite;
    public bool IsVertical;

}
