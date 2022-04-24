using UnityEngine;

public class AnimatorHelper : MonoBehaviour
{
    public void TurnOnAnimationLayer(Animator animator, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 1);
    }
    public void TurnOffAnimationLayer(Animator animator, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, 0);
    }
}
