using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class GameEventManager : MonoBehaviour
{
    static public GameEventManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }



}
