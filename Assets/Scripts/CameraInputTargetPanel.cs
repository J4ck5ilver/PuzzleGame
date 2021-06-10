using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class CameraInputTargetPanel: MonoBehaviour, IDragHandler
{

    public delegate void CameraMoved(Vector2 delta);
    public static event CameraMoved OnPlayerMovedCamera;
    public void OnDrag(PointerEventData eventData)
    {
        OnPlayerMovedCamera?.Invoke(eventData.delta);
    }

}
