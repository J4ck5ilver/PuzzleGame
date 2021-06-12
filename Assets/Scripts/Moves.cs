using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves : MonoBehaviour
{
    [SerializeField] bool playMoves = false;

    [SerializeField] bool loop = false;
    Vector3 startPos;
    Vector3 closestCenter;

    [SerializeField] float walkSpeed = 0.0f;
    [SerializeField] float jumpSpeed = 0.0f;
    [SerializeField] int currentStep = 0;
    [SerializeField] bool moveCompleted = false;

    [SerializeField] bool onGround = false;
    [SerializeField] float checkRadiusGround;
    [SerializeField] LayerMask whatIsGround;

    Dictionary<CardType, System.Action> actionAtlas = new Dictionary<CardType, System.Action>();

    Vector3 StepStartPos;
    Vector3 goalPos;

    Rigidbody rigidbody;

    [SerializeField] Card activeCard;
    Vector3 steps;

    public delegate void ActionComplete();
    public static event ActionComplete OnActionComplete;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody>();

        startPos = transform.position;

        actionAtlas.Add(CardType.Walk, Walk);
        actionAtlas.Add(CardType.Jump, Jump);

    }

    
    // Update is called once per frame
    void Update()
    {

        if (playMoves && activeCard != null)
        {
            if (moveCompleted == false)
            {
                steps = UtilsClass.DirectionToDirectionVector(activeCard.GetData().direction)* activeCard.GetData().numberOfMoves;
                actionAtlas[activeCard.GetData().type]();
            }
            else
            {
                currentStep = 0;

                if (MoveToClosetsCenter())
                {
                    moveCompleted = false;
                    currentStep = 0;
                    Debug.Log("Player: Im done");
                    //activeCard = null;
                    playMoves = false;
                    OnActionComplete();
                }
            }


        }
    }

    public void SetActiveCard(Card card)
    {
        activeCard = card;
    }

    public void SetPlay(bool state)
    {
        playMoves = state;
    }

  

    private void OnCollisionEnter(Collision collision)
    {
        if ((whatIsGround.value & 1 << collision.gameObject.layer) != 0)
        {
            onGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if ((whatIsGround.value & 1 << collision.gameObject.layer) != 0)
        {
            onGround = false;
        }
    }

    public void ResetMovement()
    {
        transform.position = startPos;
        currentStep = 0;
        rigidbody.velocity = Vector3.zero;
        closestCenter = Vector3.zero;
    }

    bool MoveToClosetsCenter()
    {
        if (currentStep == 0)
        {
           // Debug.Log("Player: My pos: " + transform.position);
            closestCenter.x = Mathf.RoundToInt(transform.position.x);
            closestCenter.y = Mathf.RoundToInt(transform.position.y);
            closestCenter.z = Mathf.RoundToInt(transform.position.z);
            currentStep = 1;
        }

        return WalkForwardsPos(closestCenter, walkSpeed);
    }

    #region Walk
    void Walk()
    {
        if (currentStep == 0)
        {
            StepStartPos = transform.position;
            goalPos = transform.position + steps;
            currentStep = 1;
        }
        else if (currentStep == 1)
        {
            float distance = new Vector2(transform.position.x - goalPos.x, transform.position.z - goalPos.z).magnitude;

            if (distance > 0.05)
            {
                Vector3 direction = steps.normalized;
                transform.Translate(direction * walkSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(goalPos.x, transform.position.y, goalPos.z);
                moveCompleted = true;
            }
        }
    }

    bool WalkForwardsPos(Vector3 destination, float speed)
    {
       // Debug.Log("Player: Walk forwad: " + destination);
        Vector3 betweenVector = new Vector3(destination.x - transform.position.x, 0.0f, destination.z - transform.position.z);
        float distance = betweenVector.magnitude;
        if (distance > 0.05)
        {
            Vector3 direction = betweenVector.normalized;

            //rigidbody.MovePosition(transform.position + direction * speed * Time.deltaTime);
            // obj.transform.position = new Vector3(currentPos.x + direction.x, obj.transform.position.y, currentPos.y + direction.y);

            transform.Translate(direction * speed * Time.deltaTime);

            return false;
        }
        else
        {
            rigidbody.MovePosition(destination);
            return true;
        }

    }

    #endregion

    #region Jump
    void Jump()
    {
        if (currentStep == 0)
        {
            StepStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            goalPos = transform.position + steps;
            goalPos.y = 0;
            currentStep = 1;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(new Vector3(0.0f, 3100.0f, 0.0f));
        }
        else if (currentStep == 1)
        {
            if (onGround == false)
            {
                currentStep = 2;
            }
        }
        else if (currentStep == 2)
        {
            goalPos.y = transform.position.y;
            WalkForwardsPos(goalPos, jumpSpeed);

            if (onGround == true)
            {
                moveCompleted = true;
            }
        }
    }
    #endregion
}
