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


        if (GUILayout.Button("Move First Child From Selected to non selected"))
        {

            cardPanel.MoveFirstChildFromSelectedToNonSelcted();
            
        }

        if (GUILayout.Button("Move First Child From non selected to Selected"))
        {
            cardPanel.MoveFirstChildFromNonSelctedToSelected();
        }
    }
}
