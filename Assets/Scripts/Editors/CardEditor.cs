using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Card card = (Card)target;
        CardDescriptor descriptor = card.GetData();

 
        EditorGUILayout.Vector3Field("Vector Direction:", descriptor.directionVector);
        EditorGUILayout.EnumPopup("Type:", descriptor.type);
        EditorGUILayout.EnumPopup("Direction:", descriptor.direction);
        EditorGUILayout.Toggle("Use Special Move", descriptor.speacialMove);

    }
}
