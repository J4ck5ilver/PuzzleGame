using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CardPanelHandler : MonoBehaviour
{
    public static CardPanelHandler Instance { get; private set; }

    [SerializeField] private int NumberOfStartSlots;

    [SerializeField] private CardPanelSO currentActiveCardPanelSO;
    [SerializeField] private Canvas canvas;
    private Transform currentActiveCardPanel;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 startPos = new Vector3(-300,0,0) + canvas.transform.position;
        Quaternion startRot =  Quaternion.identity;
        currentActiveCardPanel = Instantiate(currentActiveCardPanelSO.preFab);
        currentActiveCardPanel.gameObject.SetActive(true);
        currentActiveCardPanel.SetParent(this.transform);
        currentActiveCardPanel.SetPositionAndRotation(startPos, startRot);
        //canvas.transform = new Vector3(0,0,0);
        currentActiveCardPanel.GetComponent<CardPanel>().SetActiveGUI(currentActiveCardPanelSO);
    }

    // Update is called once per frame
    void Start()
    {
        for(int i = 0; i < NumberOfStartSlots; i++)
        {
            currentActiveCardPanel.GetComponent<CardPanel>().AddCardSlot();
        }
    }
}
