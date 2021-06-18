using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


[CustomEditor(typeof(CardPanel))]
public class CardPanelEditor : Editor
{


    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();


        CardPanel cardPanel = (CardPanel)target;

        if (GUILayout.Button("Soft Reset"))
        {
            cardPanel.SetState(CardPanelState.SoftReset);
        }

        if (GUILayout.Button("Hard Reset"))
        {
            cardPanel.SetState(CardPanelState.HardReset);
        }

        if (GUILayout.Button("Next Step"))
        {
            cardPanel.SetState(CardPanelState.NextStep);
        }
        if (GUILayout.Button("Next Card"))
        {
            cardPanel.NextCard();
        }
     
    }
}
