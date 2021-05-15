using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardSlot")]
public class CardSlotSO : ScriptableObject
{
    [SerializeField] public Sprite backgroundSprite;
    [SerializeField] public Transform preFab;
}
