using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;

public class AlpacaController : NetworkBehaviour
{
    public CharacterController characterController;
    public Animator alpacaAnimator;
    public ParticleSystem spitParticle;

    public float turnSmoothTime;

    [SyncVar] public float moveSpeed = 30;
    [SyncVar] public float ammo = 1.0f;
    [SyncVar] public float fireCost = 0.1f;
    
    private float lookAngle;
    private Quaternion targetLookDir;
    private Vector3 worldMoveDirection;
    private Vector3 mouseWorldPoint;

    public void Start()
    {
        GameManager.Instance.AddPlayer(this);
    }

    private void Move(Vector3 moveDir)
    {
        Quaternion lookDir = Quaternion.LookRotation(moveDir);
        targetLookDir = Quaternion.Slerp(targetLookDir, lookDir, turnSmoothTime);
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
        transform.rotation = targetLookDir;
    }

    [Command]
    private void Spit()
    {
        if (ammo > 0)
        {
            ammo -= fireCost;
            ClientSpit();
        }
    }

    [ClientRpc]
    private void ClientSpit()
    {
        // move me back!
        spitParticle.Play(true);
    }

    private void Update()
    {
        if (GameManager.Instance.gameHasStarted
                && isLocalPlayer)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            
            worldMoveDirection = new Vector3(moveX, 0, moveZ);
            if (worldMoveDirection != Vector3.zero)
            {
                Move(worldMoveDirection);

                // play move anim
                alpacaAnimator.SetInteger("animation", 2);
            }
            else
            {
                // play idle anim
                alpacaAnimator.SetInteger("animation", 0);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spit();
            }

            // mouse shit
            Plane p = new Plane(Vector3.up, Vector3.zero);
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0;
            if (p.Raycast(mouseRay, out enter))
            {
                mouseWorldPoint = mouseRay.GetPoint(enter);
            }

            characterController.Move(Physics.gravity * Time.deltaTime);
        }
    }
}