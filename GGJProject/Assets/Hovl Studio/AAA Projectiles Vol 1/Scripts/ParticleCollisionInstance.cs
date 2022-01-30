/*This script created by using docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine.Events;

public class ParticleCollisionInstance : MonoBehaviour
{
    public GameObject[] EffectsOnCollision;
    public float DestroyTimeDelay = 5;
    public bool UseWorldSpacePosition;
    public float Offset = 0;
    public Vector3 rotationOffset = new Vector3(0,0,0);
    public bool useOnlyRotationOffset = true;
    public bool UseFirePointRotation;
    public bool DestoyMainEffect = true;
    public UnityEvent<HitInfo> OnParticleCollisionEvent;
    
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem ps;
    private AlpacaController alpaca;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        alpaca = GetComponentInParent<AlpacaController>();
    }
    
    void OnParticleCollision(GameObject other)
    {      
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        bool hasAppliedHit = false;
        for (int i = 0; i < numCollisionEvents; i++)
        {
            // Apply Hit
            if (!hasAppliedHit)
            {
                IDamageable dmgable = other.GetComponentInParent<IDamageable>();
                if (dmgable != null)
                {
                    HitInfo hitInfo = new HitInfo(dmgable.GetNetworkIdentity().netId, alpaca.netId);
                    if (dmgable is Farmer)
                    {
                        GameManager.Instance.HitFarmer(hitInfo);
                    }
                    
                    hasAppliedHit = true;
                }
            }
            
            // Apply Effects
            foreach (var effect in EffectsOnCollision)
            {
                var instance = Instantiate(effect, collisionEvents[i].intersection + collisionEvents[i].normal * Offset, new Quaternion()) as GameObject;
                if (!UseWorldSpacePosition) instance.transform.parent = transform;
                if (UseFirePointRotation) { instance.transform.LookAt(transform.position); }
                else if (rotationOffset != Vector3.zero && useOnlyRotationOffset) { instance.transform.rotation = Quaternion.Euler(rotationOffset); }
                else
                {
                    instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
                    instance.transform.rotation *= Quaternion.Euler(rotationOffset);
                }
                Destroy(instance, DestroyTimeDelay);
            }
        }

        if (DestoyMainEffect == true)
        {
            Destroy(gameObject, DestroyTimeDelay + 0.5f);
        }
    }
}
