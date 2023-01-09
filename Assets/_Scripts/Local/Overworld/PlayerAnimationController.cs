using UnityEngine;
using Enum;

namespace Overworld
{
    public class PlayerAnimationController
    {        
        public static void AnimationUpdate(bool isGrounded, int leftRightDirection, int forwardDirection, 
            ref PlayerAnimationState animationState, Animator animator)
        {
            if (isGrounded)
            {
                switch ((leftRightDirection, forwardDirection))
                {
                    case (1, 1):
                        if(animationState != PlayerAnimationState.StrafingForwardRight)
                        {
                            animationState = PlayerAnimationState.StrafingForwardRight;
                            animator.SetTrigger("StrafeForwardRight");
                        }
                        break;
                    case (1, 0):
                        if (animationState != PlayerAnimationState.StrafingRight)
                        {
                            animationState = PlayerAnimationState.StrafingRight;
                            animator.SetTrigger("StrafeRight");
                        }                        
                        break;
                    case (1, -1):
                        if (animationState != PlayerAnimationState.StrafingBackRight)
                        {
                            animationState = PlayerAnimationState.StrafingBackRight;
                            animator.SetTrigger("StrafeBackRight");
                        }
                        break;
                    case (0, 1):
                        if (animationState != PlayerAnimationState.RunningForward)
                        {
                            animationState = PlayerAnimationState.RunningForward;
                            animator.SetTrigger("RunForward");
                        }
                        break;
                    case (0, 0):
                        if (animationState != PlayerAnimationState.Idle)
                        {
                            animationState = PlayerAnimationState.Idle;
                            animator.SetTrigger("Idle");
                        }
                        break;
                    case (0, -1):
                        if (animationState != PlayerAnimationState.RunningBack)
                        {
                            animationState = PlayerAnimationState.RunningBack;
                            animator.SetTrigger("RunBack");
                        }
                        break;
                    case (-1, 1):
                        if (animationState != PlayerAnimationState.StrafingForwardLeft)
                        {
                            animationState = PlayerAnimationState.StrafingForwardLeft;
                            animator.SetTrigger("StrafeForwardLeft");
                        }
                        break;
                    case (-1, 0):
                        if (animationState != PlayerAnimationState.StrafingLeft)
                        {
                            animationState = PlayerAnimationState.StrafingLeft;
                            animator.SetTrigger("StrafeLeft");
                        }
                        break;
                    case (-1, -1):
                        if (animationState != PlayerAnimationState.StrafingBackLeft)
                        {
                            animationState = PlayerAnimationState.StrafingBackLeft;
                            animator.SetTrigger("StrafeBackLeft");
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (animationState != PlayerAnimationState.Jumping)
                {
                    animator.SetBool("Jumping", true);
                    animationState = PlayerAnimationState.Jumping;
                }
            }
        }
    }
}
