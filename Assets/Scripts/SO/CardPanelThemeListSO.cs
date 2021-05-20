using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/CardPanelThemeList")]
public class CardPanelThemeListSO : ScriptableObject
{
    public List<CardPanelThemeSO> list;
}
