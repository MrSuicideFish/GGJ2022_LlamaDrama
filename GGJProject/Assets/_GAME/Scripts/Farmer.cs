using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Farmer : NetworkBehaviour
{
    private const int ANIM_IDLE = 0;
    private const int ANIM_RUN = 1;
    
    public Animator animator;
    public float nextRandomPositionDistThreshold = 0.3f;
    public float nextRandomPositionRange = 2.0f;
    
    private NavMeshAgent navAgent;
    private bool isMovingRandomly;
    private float moveRandomTimer;

    private void OnEnable()
    {
        navAgent = this.GetComponent<NavMeshAgent>();
    }

    [ContextMenu("Start Moving Randomly")]
    public void MoveRandomly()
    {
        Debug.Log("Start Moving Randomly");
        isMovingRandomly = true;
    }

    public void StopMovingRandomly()
    {
        Debug.Log("Stop Moving Randomly");
        isMovingRandomly = false;
    }
    
    public void MoveTo(Vector3 position)
    {
        Debug.Log("Attempting to go to position: " + position);
        if (navAgent != null)
        {
            navAgent.SetDestination(position);
            SetAnimation(ANIM_RUN);
        }
    }

    public void Stop()
    {
        if (navAgent != null)
        {
            navAgent.isStopped = true;
            SetAnimation(ANIM_IDLE);
        }
    }

    [Command]
    public void Hit(AlpacaColor hitColor)
    {
        ClientHit(hitColor);
    }

    [ClientRpc]
    private void ClientHit(AlpacaColor hitColor)
    {
        // set shader hit amount
    }

    private void Update()
    {
        if (isMovingRandomly)
        {
            float distToDestination = Vector3.Distance(transform.position, navAgent.destination);
            if (distToDestination < nextRandomPositionDistThreshold)
            {
                // choose another random destination
                Vector3 targetDest = GameManager.Instance.gameCameraDollyTrack.FollowTargetPosition;
                targetDest.x += UnityEngine.Random.Range(-nextRandomPositionRange, nextRandomPositionRange);
                targetDest.z += UnityEngine.Random.Range(-nextRandomPositionRange, nextRandomPositionRange);
                
                MoveTo(targetDest);
            }
            moveRandomTimer += Time.deltaTime;
        }
    }

    private void SetAnimation(int val)
    {
        if (animator != null)
        {
            animator.SetInteger("Animation", val);
        }
    }
}