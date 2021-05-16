using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardData : MonoBehaviour
{

    private CardDescriptor data;

    public void SetData(CardDescriptor data)
    {
        this.data = data;
    }

    public CardDescriptor GetData()
    {
        return data;
    }

}
