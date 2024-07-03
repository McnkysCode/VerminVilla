using UnityEngine;

public class IntroBehavior : StateMachineBehaviour
{
    private int rand;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rand = Random.Range(0, 3);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rand == 0)
        {
            animator.SetTrigger("Run1");
        }
        else if (rand == 1)
        {
            animator.SetTrigger("Jump1");
        }
        else
        {
            animator.SetTrigger("Idle1");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}