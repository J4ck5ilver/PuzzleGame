using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


[CustomEditor(typeof(CardGenerator))]
public class CardGeneratorEditor : Editor
{



    static private CardDescriptor cardDescriptor = new CardDescriptor();

    //private int numberOfMoves;
    //private Vector3 direction;
    //private bool speacialMove;
    //private CardType type;



    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();

       
       CardGenerator cardGenerator = (CardGenerator)target;

        cardDescriptor.numberOfMoves = EditorGUILayout.IntField("Number of moves:", cardDescriptor.numberOfMoves);
        cardDescriptor.numberOfMoves = Mathf.Clamp(cardDescriptor.numberOfMoves, GameConstants.minNumberOfMoves, GameConstants.maxNumberOfMoves);
        cardDescriptor.directionVector = EditorGUILayout.Vector3Field("Vector Direction:", cardDescriptor.directionVector);
        cardDescriptor.type = (CardType)EditorGUILayout.EnumPopup("Type:", cardDescriptor.type);
        cardDescriptor.direction = (Direction)EditorGUILayout.EnumPopup("Direction:", cardDescriptor.direction);
        cardDescriptor.speacialMove = EditorGUILayout.Toggle("Use Special Move", cardDescriptor.speacialMove);

        if (GUILayout.Button("Generate Card"))
        {
            CardDescriptor newDesc = new CardDescriptor(cardDescriptor);
            cardGenerator.CreateCard(newDesc);
            
        }
    }
}
