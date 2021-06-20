using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassButton : MonoBehaviour
{


    private void Awake()
    {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(ButtonClicked);
    }

    

    private void ButtonClicked()
    {
        CameraManager.Instance.InterpolateCameraToClosestLatitude();
    }
}
