using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{



    private static Camera mainCamera;


    public static Canvas GetTopmostCanvas(Component component)
    {
        Canvas[] parentCanvases = component.GetComponentsInParent<Canvas>();
        if (parentCanvases != null && parentCanvases.Length > 0)
        {
            return parentCanvases[parentCanvases.Length - 1];
        }
        return null;
    }

    public static Color GetDirectionColor(Direction direction)
    {
        Color returnValue = Color.white;
        switch(direction)
        {
            case Direction.North:
                returnValue = Color.red;
                break;
            case Direction.West:
                returnValue = new Color(1.0f,0.7f,0.0f);
                break;
            case Direction.South:
                returnValue = Color.blue;
                break;
            case Direction.East:
                returnValue = Color.green;
                break;
            case Direction.None:
                returnValue = Color.black;
                break;
        }


        return returnValue;
    }

    public static float DirectionToDegrees(Direction direction)
    {
        float returnValue = 0;
        switch(direction)
        {
            case Direction.North:
                returnValue = 0;
                break;
            case Direction.West:
                returnValue = 90;
                break;
            case Direction.South:
                returnValue = 180;
                break;
            case Direction.East:
                returnValue = 270;
                break;
            case Direction.None:
                returnValue = 1337;
                break;

        }
        return returnValue;
    }


    public static Direction DegreesToDirection(float degress)
    {
        Direction returnValue = Direction.None;

        System.Int32 degreesAsInt = Mathf.RoundToInt(degress);

        switch (degreesAsInt)
        {
            case 0:
                returnValue = Direction.North;
                break;
            case 90:
                returnValue = Direction.West;
                break;
            case 180:
                returnValue = Direction.South;
                break;
            case 270:
                returnValue = Direction.East;
                break;
        }
        return returnValue;
    }


    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        //Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //mouseWorldPosition.z = 0f;
        //return mouseWorldPosition;



        var mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCamera.transform.position.z); // select distance = 10 units from the camera
       // Debug.Log(mainCamera.ScreenToWorldPoint(mousePos));
        return mousePos;

    }

    public static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f).normalized;
    }

    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = Mathf.Rad2Deg * radians;
        return degrees;
    }

}
