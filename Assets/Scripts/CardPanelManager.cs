using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CardPanelManager : MonoBehaviour
{
    public static CardPanelManager Instance { get; private set; }

    private Transform startPanel;
    private Transform endPanel;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        startPanel = transform.Find("startPanel");
        endPanel = transform.Find("endPanel");


    }

}
