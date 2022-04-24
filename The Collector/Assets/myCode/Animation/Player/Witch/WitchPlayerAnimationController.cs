using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class WitchPlayerAnimationController : MonoBehaviour
{
    [SerializeField] private bool crawlCutscene;
    [SerializeField] private bool idleCutscene;
    public Animator witchPlayerAnimator;
    private AnimatorHelper animatorHelper;
    public GameObject interactionObj;
    public GroundItem interactionItem;
    public Transform itemCarryTransform;
    void Start()
    {
        witchPlayerAnimator = GetComponent<Animator>();
        animatorHelper = GetComponent<AnimatorHelper>();
        animatorHelper.TurnOffAnimationLayer(witchPlayerAnimator, 1);
        if(crawlCutscene)
        {
            UpdateAnimationsOnCrawl(true);
            UpdateAnimationsOnMovement(new Vector2(0, 3));
        }
        else if (idleCutscene)
        {
            UpdateAnimationsOnCrawl(false);
            UpdateAnimationsOnMovement(Vector2.zero);
        }
    }

    void Update()
    {
        
    }

    
    public void UpdateAnimationsOnMovement(Vector2 currentInput)
    {
        bool isStrafing = Mathf.Abs(currentInput.x) > 0;
        witchPlayerAnimator.SetBool("IsStrafing", isStrafing);
        witchPlayerAnimator.SetFloat("CurrentInput", isStrafing ? currentInput.x : currentInput.y);
    }

    public void UpdateAnimationsOnCrouch(bool toCrouch)
    {
       
        witchPlayerAnimator.SetBool("IsCrouching", toCrouch);
        //witchPlayerAnimator.SetBool("IsCrawling", !toCrouch);
    }
    public void UpdateAnimationsOnCrawl(bool toCrawl)
    {
        witchPlayerAnimator.SetBool("IsCrawling", toCrawl);
        //witchPlayerAnimator.SetBool("IsCrouching", !toCrawl);
    }
    public void AnimatorOnStartMovingItem()
    {
        animatorHelper.TurnOnAnimationLayer(witchPlayerAnimator,1);
        witchPlayerAnimator.SetTrigger("StartMovingItem");
        //Destroy(interactionObj);
    }
    private void PutItemInHands()
    {
        //interactionItem
        interactionObj.transform.SetParent(itemCarryTransform);
        interactionObj.transform.localPosition = Vector3.zero;
    }
    public void AnimatorOnDoneMoving()
    {
        witchPlayerAnimator.SetTrigger("DoneMovingItem");
        interactionObj.transform.parent = null;
    }
    public void AnimatorOnStartAttack(int whichHand)
    {
       animatorHelper.TurnOnAnimationLayer(witchPlayerAnimator, 1);
       if(whichHand == 0)
        {
            witchPlayerAnimator.SetTrigger("AttackLeft");
        }
       else if(whichHand == 1)
        {
            witchPlayerAnimator.SetTrigger("AttackRight");
        }
    }
    private async void AnimatorOnDoneAttackingLeft()
    {
        witchPlayerAnimator.SetTrigger("DoneAttackingLeft");
        await Task.Delay(250);
        animatorHelper.TurnOffAnimationLayer(witchPlayerAnimator, 1);
    }

    private async void AnimatorOnDoneAttackingRight()
    {
        witchPlayerAnimator.SetTrigger("DoneAttackingRight");
        await Task.Delay(250);
        animatorHelper.TurnOffAnimationLayer(witchPlayerAnimator, 1);
    }

    private void HitPlayer(FirstPersonPlayer playerTarget)
    {
       
    }




}
