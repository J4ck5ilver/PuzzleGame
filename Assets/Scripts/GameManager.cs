using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] static bool pressedPlay = false;
    [SerializeField] static bool roundStarted = false;
    [SerializeField] static bool roundComplete = true;
    [SerializeField] static bool turnComplete = true;


    [SerializeField] static Card activePlayerCard;

    public delegate void NewTurn();
    public static event NewTurn OnNewTurn;

    public delegate void TurnCompleted();
    public static event TurnCompleted OnTurnCompleted;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        CardPanel.OnPlay += UserPressedPlay;
        Moves.OnActionComplete += ActorFinishedAction;
        Moves.OnReachedGoal += PlayerReachedGoal;
    }

    void UserPressedPlay()
    {
        pressedPlay = true;
    }

    void ActorFinishedAction()
    {
        turnComplete = true;
        OnTurnCompleted();
    }

    void PlayerReachedGoal()
    {
        Debug.Log("GM: Player reached goal");

        roundComplete = true;
        roundStarted = false;
    }


    private void Update()
    {
        if(pressedPlay)
        {
            Debug.Log("GM: Pressed play");
            pressedPlay = false;
            roundStarted = true;
            turnComplete = true;
        }

        if(roundStarted)
        {

            if (turnComplete)
            {
                Debug.Log("GM:New Turn");
                turnComplete = false;
                

                // Get new Card
                activePlayerCard = CardManager.Instance.GetCurrentCardInPlay();

                if (OnNewTurn != null && activePlayerCard != null)
                {
                    OnNewTurn();
                }
                else 
                {
                    Debug.Log("GM: No more cards");
                    //End round
                }
            }
        }
    }

    public Card GetActivePlayerCard()
    {
        Debug.Log("GM: Give new card to player");

        return activePlayerCard;
    }
    
}
