using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Farmer : NetworkBehaviour
{
    private NavMeshAgent navAgent;
    
    private void OnEnable()
    {
        navAgent = this.GetComponent<NavMeshAgent>();
    }

    [Server]
    private void BeginMoveRandomly()
    {
        
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

    public void MoveTo(Vector3 position)
    {
        if (navAgent != null)
        {
            navAgent.SetDestination(position);
        }
    }
}