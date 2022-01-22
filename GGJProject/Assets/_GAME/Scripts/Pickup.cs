using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : NetworkBehaviour
{
    public UnityEvent<AlpacaController> OnPickup;
    
    [Server]
    private void OnTriggerEnter(Collider other)
    {
        AlpacaController player = other.gameObject.GetComponent<AlpacaController>();
        if (player != null)
        {
            DoPickup(player);
        }
    }

    [ClientRpc]
    protected virtual void DoPickup(AlpacaController player)
    {
        OnPickup?.Invoke(player);
    }
}
