using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] LayerMask triggedBy;
    private bool isTriggered = false;

    public bool IsTriggered()
    {
        return isTriggered;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((triggedBy.value & 1 << other.gameObject.layer) != 0)
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((triggedBy.value & 1 << other.gameObject.layer) != 0)
        {
            isTriggered = false;
        }
    }
}
