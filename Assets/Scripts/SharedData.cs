using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public enum CardType
{
    Walk,
    Jump,
    Roll,
    Idle
}

public enum CardSortOrder
{
    CardType,
    Direction,
    NumberOfMoves
}

// add themes? cards sos, + lists
public enum Direction
{
    North,
    East,
    South,
    West,
    None
}

public enum CardTheme
{
    None,
    Wood
}
public enum CardPanelTheme
{
    None,
    Wood
}

public enum CardPanelState
{
    Play,
    Select,
    Reset,
    NextStep
}

public class PanelEventArgs : EventArgs
{
    public Transform senderTransform = null;
    public PointerEventData pointerData = null;
    public Vector2 poitionData2D = new Vector2();
    public float floatData;
    public int intData;
}

public static class GameConstants
{
    public const int maxNumberOfMoves = 10;
    public const int minNumberOfMoves = 0;
}

[Serializable] public class CardDescriptor
{

    public int numberOfMoves        { get; set; }
    public Vector3 directionVector  { get; set; }
    public bool speacialMove        { get; set; }
    public CardType type            { get; set; }
    public Direction direction      { get; set; }

    public CardDescriptor()
    {
        numberOfMoves = 99;
        directionVector = new Vector3(0,0,0);
        speacialMove = false;
        type = CardType.Walk;
        direction = Direction.North;
    }
    public CardDescriptor(CardDescriptor other)
    {
        numberOfMoves = other.numberOfMoves;
        directionVector = new Vector3(other.directionVector.x, other.directionVector.y, other.directionVector.z);
        speacialMove = other.speacialMove;
        type = other.type;
        direction = other.direction;
    }
}


