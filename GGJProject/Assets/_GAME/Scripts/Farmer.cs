using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEditor.Animations;
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
    public float nextRandomPositionZOffset = 10.0f;
    public float chanceToFall = 0.2f;

    private NavMeshAgent navAgent;

    private void OnEnable()
    {
        navAgent = this.GetComponent<NavMeshAgent>();
        FarmerManager.RegisterFarmer(this);
    }

    public void MoveToRandom()
    {
        if (animator == null) return;
        
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Idle") || state.IsName("Running"))
        {
            float rnd = UnityEngine.Random.Range(0.0f, 1.0f);
            if (rnd < 0.05f)
            {
                FallDown();
                return;
            }

            // choose another random destination
            CinemachinePathBase cameraPath = GameManager.Instance.gameCameraDollyTrack.m_Path;
            Vector3 targetDest = cameraPath.EvaluatePosition(GameManager.Instance.dollyTrackPosition);
            targetDest.x += UnityEngine.Random.Range(-nextRandomPositionRange, nextRandomPositionRange);
            targetDest.z += nextRandomPositionZOffset + UnityEngine.Random.Range(0, nextRandomPositionRange);

            MoveTo(targetDest);
        }
        else
        {
            Stop();
        }
    }

    [ContextMenu("Fall Down")]
    public void FallDown()
    {
        Stop();
        StartCoroutine(FallDownCallback());
    }

    private IEnumerator FallDownCallback()
    {
        if (animator == null) yield break;
        
        animator.SetTrigger("FallDown");
        SetAnimation(ANIM_IDLE);

        yield return new WaitForSeconds(1.0f);

        animator.ResetTrigger("FallDown");

        yield return null;
    }

    public void MoveTo(Vector3 position)
    {
        if (navAgent != null)
        {
            navAgent.isStopped = false;
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

    private void SetAnimation(int val)
    {
        if (animator != null)
        {
            animator.SetInteger("Animation", val);
        }
    }
}