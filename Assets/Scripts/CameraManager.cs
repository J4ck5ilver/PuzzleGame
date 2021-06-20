using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CameraManager : MonoBehaviour
{

    public static CameraManager Instance { get; private set; }



    private const float CameraSpeed = 0.01f;
    private const float CameraInterpolationSpeed = 0.1f;
    private const float DotInterruptTollerance = 0.99f;
    CinemachineDollyCart DollyCart;//= new CinemachineDollyCart();

    //private float DollyCartTargetPos = 0.0f;
    //private float DollyRollingDirection = 0.0f;

    //private float DollyClockwiseDirection = 0.0f;
    //private float DollyCounterClockwiseDirection = 0.0f;

    private float DollyCartDirection = 0.0f;

    //Dictionary<Direction, float> DollyCartDirectionTargets;
    private Direction CurrentDirection;
    //private Direction TargetDirection;
    private Direction LastDirection;


    bool InterPolateToClosestLatitude = false;


    public event EventHandler<LatitudeChangeArgs> OnChangedLatitudeEvent;


    private Transform LookAtTarget;

    private void Awake()
    {
        Instance = this;
        DollyCart = new CinemachineDollyCart();
        LastDirection = Direction.None;

        
    }



    void Start()
    {
        DollyCart = transform.Find("Cart").GetComponent<CinemachineDollyCart>();

     //   DollyCartTargetPos = DollyCart.m_Position;
        InitLookAtTarget();
        UpdateCurrentDirection();
        //InitDollyCartDirectionValues();
        CameraInputTargetPanel.OnPlayerMovedCamera += OnPlayerCameraInput;
    }

    private void Update()
    {
        UpdateDollyCart();
        //    UpdateCurrentDirection();
    }

    private void InitLookAtTarget()
    {
        foreach (Transform child in DollyCart.transform)
        {
            if (child.name == "LookAtTarget")
            {
                LookAtTarget = child;
                break;
            }
        }
    }

    //private void InitDollyCartDirectionValues()
    //{
    //    float startPosition = DollyCart.m_Position;
    //    //  DollyCartDirectionTargets = new Dictionary<Direction, float>();

    //    for (int i = 0; i < 4; i++)
    //    {
    //        //    DollyCartDirectionTargets[(Direction)(i)] = 0.0f;




    //    }


    //    DollyClockwiseDirection = 0.0f;
    //    DollyCounterClockwiseDirection = 0.0f;

    //    // Vector3.forward;
    //    // Vector3.right

    //}

    private Vector3 GetCameraDirection()
    {
        return Camera.main.transform.position - LookAtTarget.position;
    }

    private void UpdateCurrentDirection()
    {

        Vector3 DirVector = GetCameraDirection();

        DirVector.y *= 0.0f;
        DirVector.Normalize();

        CurrentDirection = GetClosestDirectionFromVector(DirVector);


        if(CurrentDirection != LastDirection)
        {
            LastDirection = CurrentDirection;
            SendLatitideChangeEvent();
        }


        //Debug.Log(CurrentDirection);

        //CurrentDirection;
    }

    private void SendLatitideChangeEvent()
    {
        LatitudeChangeArgs eventArgs = new LatitudeChangeArgs();
        eventArgs.Latitude = CurrentDirection;
        OnChangedLatitudeEvent?.Invoke(this, eventArgs);
    }

    private void UpdateDollyCart()
    {
        if (InterPolateToClosestLatitude)
        {



            //float startPosition = DollyCart.m_Position;
            ////DollyCartDirectionTargets = new Dictionary<Direction, float>();

            //for (int i = 0; i < 4; i++)
            //{
            //    //     DollyCartDirectionTargets[(Direction)(i)] = 0.0f;

            //}

            //float DollyCartDirection = CameraInterpolationSpeed;

            Direction closestDirection;

            Vector3 dirVector = GetCameraDirection();
            dirVector.y *= 0.0f;
            dirVector.Normalize();

            closestDirection = GetClosestDirectionFromVector(dirVector);

            float startPosition = DollyCart.m_Position;
            float startDot = Vector3.Dot(UtilsClass.DirectionToDirectionVector(closestDirection), dirVector);

            if (startDot != 1.0f)
            {

                Vector3 diff1 = DollyCart.transform.position;

                const float cartPushValue = 0.5f;
                DollyCart.m_Position += cartPushValue;

                Debug.Log((diff1 - DollyCart.transform.position).magnitude);

                Vector3 newdirVector = GetCameraDirection();
                newdirVector.y *= 0.0f;
                newdirVector.Normalize();
                float newDot = Vector3.Dot(UtilsClass.DirectionToDirectionVector(closestDirection), newdirVector);
                DollyCartDirection = (startDot < newDot) ? 1.0f : -1.0f;

            }


            DollyCart.m_Position = startPosition;






            if (DollyCartDirection != 0.0f)
            {
                float currentDot = Vector3.Dot(dirVector,UtilsClass.DirectionToDirectionVector(closestDirection));

                if(currentDot < DotInterruptTollerance)
                {
                    DollyCart.m_Position += DollyCartDirection * CameraInterpolationSpeed;
                }
                else
                {
                    InterPolateToClosestLatitude = false;
                }
            }

            else
            {
                InterPolateToClosestLatitude = false;
            }

            UpdateCurrentDirection();
        }
    }

    private void OnPlayerCameraInput(Vector2 delta)
    {
        DollyCart.m_Position += delta.x * CameraSpeed;
        UpdateCurrentDirection();
        InterPolateToClosestLatitude = false;
    }

    public void InterpolateCameraToClosestLatitude()
    {
        //Direction closestDirection;

        //Vector3 dirVector = Camera.main.transform.position - LookAtTarget.position;
        //dirVector.y *= 0.0f;
        //dirVector.Normalize();

        //closestDirection = GetClosestDirectionFromVector(dirVector);
        //DollyCartDirection = GetDollyCartDirectionTowardsGoal(closestDirection, dirVector);

        
        InterPolateToClosestLatitude = true;

        // ScreenMovement;
        //Direction nextDirection = GetClosestDirectionFromVector(DirVector,closestDirection);



        //Vector3 tmpDirection = UtilsClass.DirectionToDirectionVector((Direction)i);
        //Vector3 calcVector = new Vector3();
        //calcVector = DirVector;
        //calcVector.y *= 0.0f;
        //float DotProduct = Vector3.Dot(calcVector, tmpDirection);
        //float angle = Mathf.Acos(DotProduct) / (calcVector.magnitude * tmpDirection.magnitude);




        // Vector3 

    }

    //private float GetDollyCartDirectionTowardsGoal(Direction targetDirection, Vector3 currentDirection)
    //{

    //    float returnValue = 0.0f;
    //    float startPosition = DollyCart.m_Position;
    //    //Vector3 dirVectorStart = Camera.main.transform.position - LookAtTarget.position;

    //    //float xSideStart = currentDirection.x;
    //    //float ySideStart = currentDirection.y;

    //    float startDot = Vector3.Dot(UtilsClass.DirectionToDirectionVector(targetDirection), currentDirection);


    //    if (startDot != 1.0f)
    //    {

    //        const float cartPushValue = 0.1f;
    //        DollyCart.m_Position += cartPushValue;
    //        Vector3 newdirVector = Camera.main.transform.position; //- LookAtTarget.position;
    //        newdirVector.y *= 0.0f;
    //        newdirVector.Normalize();
    //        float newDot = Vector3.Dot(UtilsClass.DirectionToDirectionVector(targetDirection), newdirVector);
    //        returnValue = (startDot < newDot) ? 1.0f : -1.0f;

    //    }


    //    DollyCart.m_Position = startPosition;

    //    return returnValue;
    //}

    private Direction GetClosestDirectionFromVector(Vector3 direction, Direction ExcludeDirection = Direction.None)
    {

        Direction closestDirection = Direction.None;

        float closestDot = -1.1f;
        Direction tmpDirection;

        for (int i = 0; i < 4; i++)
        {
            tmpDirection = (Direction)i;

            Vector3 tmpDirectionVector = UtilsClass.DirectionToDirectionVector(tmpDirection);
            Vector3 calcVector = new Vector3();
            calcVector = direction;
            calcVector.y *= 0.0f;
            float DotProduct = Vector3.Dot(calcVector, tmpDirectionVector);



            if (DotProduct > closestDot && tmpDirection != ExcludeDirection)
            {
                closestDot = DotProduct;
                closestDirection = tmpDirection;
            }
        }

        return closestDirection;
    }



}
