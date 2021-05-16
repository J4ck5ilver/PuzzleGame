using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{



    private static Camera mainCamera;



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
 
        }
        return returnValue;
    }


    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
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
