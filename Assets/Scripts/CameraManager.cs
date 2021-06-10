using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{

    private const float CameraSpeed = 0.01f;
    CinemachineDollyCart DollyCart = new CinemachineDollyCart();

    void Start()
    {
        DollyCart = transform.Find("Cart").GetComponent<CinemachineDollyCart>();
        CameraInputTargetPanel.OnPlayerMovedCamera += UpdateCamera;
    }

    private void UpdateCamera(Vector2 delta)
    {
        DollyCart.m_Position += delta.x * CameraSpeed;
    }
    

}
