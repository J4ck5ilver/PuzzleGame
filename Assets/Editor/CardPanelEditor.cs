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

        if (GUILayout.Button("Reset State"))
        {
            cardPanel.SetState(CardPanelState.Reset);
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
