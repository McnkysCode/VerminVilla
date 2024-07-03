using UnityEngine;

public class InspawnBehavior : StateMachineBehaviour
{
    private BoxCollider boxCollider;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the BoxCollider component on the Animator's GameObject
        boxCollider = animator.GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            // Disable the BoxCollider
            boxCollider.enabled = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boxCollider != null)
        {
            // Enable the BoxCollider
            boxCollider.enabled = true;
        }
    }
}