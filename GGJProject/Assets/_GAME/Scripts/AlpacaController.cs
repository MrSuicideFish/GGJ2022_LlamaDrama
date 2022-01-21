using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AlpacaController : MonoBehaviour
{
    public CharacterController characterController;
    public CinemachineVirtualCamera playerCamera;

    private void Start()
    {
        playerCamera.transform.SetParent(null, true);
    }
}
