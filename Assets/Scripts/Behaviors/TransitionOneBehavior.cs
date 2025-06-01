using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionOneBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    CombatManager.instance.canReceiveInput = true;
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerMovement.instance.isAttacking)
        {
            if (PlayerMovement.instance.isGrounded)
            {
                PlayerMovement.instance.animator.Play("Attack2");
            }

            //animator.SetTrigger("AttackTwo");
            //PlayerMovement.instance.animator.Play("Attack2");
            //CombatManager.instance.inputReceived = false;
        }
        else if (PlayerMovement.instance.IsAirAttacking && !PlayerMovement.instance.isGrounded)
        {
            PlayerMovement.instance.animator.Play("Air_Attack_02");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement.instance.isAttacking = false;
        PlayerMovement.instance.IsAirAttacking = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
