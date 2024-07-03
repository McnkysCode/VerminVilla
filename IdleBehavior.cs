using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    public float Timer;
    private int Rand;

    public float minTime = 2;
    public float maxTime = 4;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rand = Random.Range(0, 2);
        Timer = Random.Range(minTime, maxTime);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Timer <= 0)
        {
            if (Rand == 0)
            {
                animator.SetTrigger("Run1");
            }
            else
            {
                animator.SetTrigger("Jump1");
            }
        }
        else
        {
            Timer -= Time.deltaTime;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}