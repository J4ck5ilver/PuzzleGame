using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


[CustomEditor(typeof(CardGenerator))]
public class CardGeneratorEditor : Editor
{
    static private CardDescriptor cardDescriptor = new CardDescriptor();

    static int numberOfRandomCards;

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
        if (GUILayout.Button("Remove Cards"))
        {
            foreach(Transform child in cardGenerator.transform)
            {
                CardManager cardManager = child.gameObject.GetComponent<CardManager>();
                if(cardManager != null)
                {
                    cardManager.RemoveAllStartCards();
                }

            }
        }
            numberOfRandomCards = EditorGUILayout.IntField("Number of moves:", numberOfRandomCards);
        if (GUILayout.Button("Generate Number of Random Debug Cards"))
        {
            for (int i = 0; i < numberOfRandomCards; i++)
            {
                CardDescriptor newDesc = new CardDescriptor();

                newDesc.direction = (Direction)Random.Range(0,5);
                newDesc.numberOfMoves = Random.Range(0,10);
                newDesc.type = (CardType)Random.Range(0, 3);
                cardGenerator.CreateCard(newDesc);
            }
        }
    }
}
