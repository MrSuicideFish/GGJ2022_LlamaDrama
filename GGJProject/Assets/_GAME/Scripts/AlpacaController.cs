using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;

public class AlpacaController : NetworkBehaviour
{
    public const int InteractionLayer = 10;
    public const int ANIMATION_IDLE = 0;
    public const int ANIMATION_MOVE = 2;
    public const int ANIMATION_DRINK = 4;
    
    public CharacterController characterController;
    public Animator alpacaAnimator;
    public ParticleSystem spitParticle;

    public float turnSmoothTime;
    public float interactRaycastDist = 3.0f;

    [SyncVar] public float moveSpeed = 30;
    [SyncVar] public float ammo = 1.0f;
    [SyncVar] public float fireCost = 0.1f;
    [SyncVar] public bool isDrinking = false;
    
    private float lookAngle;
    private Quaternion targetLookDir;
    private Vector3 worldMoveDirection;
    private Vector3 mouseWorldPoint;

    public void Start()
    {
        GameManager.Instance.AddPlayer(this);
    }

    [Command]
    public void GiveAmmo()
    {
        ClientGiveAmmo();
    }

    [Command]
    public void BeginDrink(Vector3 position, Vector3 direction)
    {
        ClientBeginDrink(position, direction);
    }

    [Command]
    public void EndDrink()
    {
        ClientEndDrink();
    }

    [ClientRpc]
    private void ClientBeginDrink(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.forward = direction;
        isDrinking = true;
    }

    [ClientRpc]
    private void ClientEndDrink()
    {
        isDrinking = false;
    }

    [ClientRpc]
    private void ClientGiveAmmo()
    {
        ammo = 1.0f;
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
        // do gravity
        characterController.Move(Physics.gravity * Time.deltaTime);

        if (GameManager.Instance.gameHasStarted
                && isLocalPlayer)
        {
            // detect use
            Ray fwdRay = new Ray(transform.position, transform.forward);
            RaycastHit interactionHit;
            if (Physics.Raycast(fwdRay, out interactionHit, interactRaycastDist, 1 << InteractionLayer ))
            {
                Debug.Log("Found interact layer!");
                
                if (Input.GetKeyDown(KeyCode.Space)
                    || Input.GetMouseButtonDown(0))
                {
                    IUseable useable = interactionHit.transform.gameObject.GetComponent<IUseable>();
                    if (useable != null)
                    {
                        useable.Use(this);
                        return;
                    }
                }
            }
            
            // mouse shit
            Plane p = new Plane(Vector3.up, Vector3.zero);
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0;
            if (p.Raycast(mouseRay, out enter))
            {
                mouseWorldPoint = mouseRay.GetPoint(enter);
            }
            
            if (isDrinking)
            {
                // play drink anim
                alpacaAnimator.SetInteger("animation", ANIMATION_DRINK);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) 
                    || Input.GetMouseButtonDown(0))
                {
                    Spit();
                }
                
                worldMoveDirection = new Vector3(
                    Input.GetAxis("Horizontal"),
                    0,
                    Input.GetAxis("Vertical"));
                
                if (worldMoveDirection != Vector3.zero)
                {
                    Move(worldMoveDirection);

                    // play move anim
                    alpacaAnimator.SetInteger("animation", ANIMATION_MOVE);
                }
                else
                {
                    // play idle anim
                    alpacaAnimator.SetInteger("animation", ANIMATION_IDLE);
                }
            }
        }
    }
}