using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CardType
{
    Walk,
    Jump,
    Roll,
    Idle
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



public class CardDescriptor
{
    public int numberOfMoves                 { get; set; }
    public Vector3 directionVector  { get; set; }
    public bool speacialMove        { get; set; }
    public CardType type            { get; set; }
    public Direction direction      { get; set; }

    public CardDescriptor()
    {
        numberOfMoves = 0;
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


