using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class FarmerManager : NetworkBehaviour
{
    private static List<Farmer> allFarmers = new List<Farmer>();
    private float redirectTimer;
    public float timeToRefreshDestination = 1;
    private bool hasStarted;

    public static void RegisterFarmer(Farmer farmer)
    {
        if (allFarmers == null)
        {
            allFarmers = new List<Farmer>();
        }
        
        allFarmers.Add(farmer);
    }

    public Farmer GetFarmerById(uint id)
    {
        return allFarmers.FirstOrDefault(x => x.netId == id);
    }
    
    private void Update()
    {
        if (netIdentity.isServer
            && GameManager.Instance != null 
            && GameManager.Instance.gameHasStarted)
        {
            if (hasStarted)
            {
                redirectTimer += Time.deltaTime;
                if (redirectTimer > 0.5f)
                {
                    foreach (var f in allFarmers)
                    {
                        f.MoveToRandom();
                    }
                
                    redirectTimer = 0.0f;
                }
            }
            else
            {
                foreach (var f in allFarmers)
                {
                    f.MoveToRandom();
                }

                hasStarted = true;
            }
        }
    }
}
