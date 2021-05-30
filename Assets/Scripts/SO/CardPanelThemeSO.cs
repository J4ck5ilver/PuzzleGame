using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/CardPanelTheme")]
public class CardPanelThemeSO : ScriptableObject
{
    public Sprite panelBackgroundSprite;
    public Sprite panelBorderSprite;
    public CardSlotThemeSO cardSlotTheme;
    public CardPanelTheme theme;
}
