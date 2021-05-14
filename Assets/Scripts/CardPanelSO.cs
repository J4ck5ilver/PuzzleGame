using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/CardPanel")]
public class CardPanelSO : ScriptableObject
{

    public Transform preFab;
    public Sprite panelBackgroundSprite;
    public Sprite panelBorderSprite;
    public Sprite cardSlotBackgroundSprite;
    public Sprite cardSlotBorderSprite;
}
