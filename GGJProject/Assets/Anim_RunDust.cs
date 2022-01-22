using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_RunDust : StateMachineBehaviour
{
    private ParticleSystem dustParticle;

    private ParticleSystem GetDustParticle(Animator animator)
    {
        if (dustParticle == null)
        {
            AlpacaController alpaca = animator.GetComponentInParent<AlpacaController>();
            if (alpaca != null)
            {
                dustParticle = alpaca.dustParticle;
            }
        }
        return dustParticle;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GetDustParticle(animator) != null)
        {
            dustParticle.Play(true);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GetDustParticle(animator) != null)
        {
            dustParticle.Play(true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GetDustParticle(animator) != null)
        {
            dustParticle.Stop(true);
        }
    }
}
