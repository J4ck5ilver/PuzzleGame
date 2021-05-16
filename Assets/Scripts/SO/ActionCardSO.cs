using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActionCard")]
public class ActionCardSO : ScriptableObject
{
    public Sprite sprite;
    public CardType type;
}
