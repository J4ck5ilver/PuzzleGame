using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCardSlot : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Card Inside HitBox");
    }

}
