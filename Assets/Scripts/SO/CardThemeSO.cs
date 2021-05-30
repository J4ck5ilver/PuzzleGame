using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardTheme")]
public class CardThemeSO : ScriptableObject
{
    [SerializeField] public Sprite backgroundSprite;
   public  CardTheme theme;
}
