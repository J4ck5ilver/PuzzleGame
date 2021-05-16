using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/ActionCardsList")]
public class ActionCardsListSO : ScriptableObject
{
    public List<ActionCardSO> list;
}
