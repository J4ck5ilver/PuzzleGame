using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/CardThemeList")]
public class CardThemeListSO : ScriptableObject
{
    public List<CardThemeSO> list;
}
