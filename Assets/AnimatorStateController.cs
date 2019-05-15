using UnityEngine;

public class AnimatorStateController : StateMachineBehaviour
{
    public bool enter;
    public bool exit;
    public bool setAttackBool;

    // This will be called once the animator has transitioned out of the state.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (exit)
            return;
        FindObjectOfType<StasisCharacter>().anim.SetBool("attacking", setAttackBool);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (enter)
            return;
        FindObjectOfType<StasisCharacter>().anim.SetBool("attacking", setAttackBool);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (exit)
            return;
        FindObjectOfType<StasisCharacter>().anim.SetBool("attacking", setAttackBool);
    }
}
